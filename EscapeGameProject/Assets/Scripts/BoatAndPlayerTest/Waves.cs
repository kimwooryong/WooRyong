using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour
{
    [Tooltip("매쉬 간격")]
    public int Dimension = 10;
    [Tooltip("UV 크기")]
    public float UVScale = 2f;
    [Tooltip("옥타브 설정")]
    public Octave[] Octaves;

    // Mesh 설정
    protected MeshFilter MeshFilter;
    protected Mesh Mesh;

    //



    // Start is called before the first frame update
    void Start()
    {
        // 메쉬 세팅
        Mesh = new Mesh();
        Mesh.name = gameObject.name;

        Mesh.vertices = GenerateVerts();
        Mesh.triangles = GenerateTries();
        Mesh.uv = GenerateUVs();
        Mesh.RecalculateNormals();
        Mesh.RecalculateBounds();

        MeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshFilter.mesh = Mesh;


    }

    public float GetHeight(Vector3 position)
    {
        // 로컬 공간에서의 스케일 요소와 위치
        var scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        var localPos = Vector3.Scale((position - transform.position), scale);

        //꼭짓점 얻기
        var p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        var p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        var p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        var p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        //clamp if the position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, Dimension);
        p1.z = Mathf.Clamp(p1.z, 0, Dimension);
        p2.x = Mathf.Clamp(p2.x, 0, Dimension);
        p2.z = Mathf.Clamp(p2.z, 0, Dimension);
        p3.x = Mathf.Clamp(p3.x, 0, Dimension);
        p3.z = Mathf.Clamp(p3.z, 0, Dimension);
        p4.x = Mathf.Clamp(p4.x, 0, Dimension);
        p4.z = Mathf.Clamp(p4.z, 0, Dimension);

        //get the max distance to one of the edges and take that to compute max - dist
        var max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        var dist = (max - Vector3.Distance(p1, localPos))
                 + (max - Vector3.Distance(p2, localPos))
                 + (max - Vector3.Distance(p3, localPos))
                 + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        //weighted sum
        var height = Mesh.vertices[index(p1.x, p1.z)].y * (max - Vector3.Distance(p1, localPos))
                   + Mesh.vertices[index(p2.x, p2.z)].y * (max - Vector3.Distance(p2, localPos))
                   + Mesh.vertices[index(p3.x, p3.z)].y * (max - Vector3.Distance(p3, localPos))
                   + Mesh.vertices[index(p4.x, p4.z)].y * (max - Vector3.Distance(p4, localPos));

        //scale
        return height * transform.lossyScale.y / dist;

    }

    private Vector3[] GenerateVerts()
    {
        var verts = new Vector3[(Dimension + 1) * (Dimension + 1)];

        //equaly distributed verts
        for (int x = 0; x <= Dimension; x++)
            for (int z = 0; z <= Dimension; z++)
                verts[index(x, z)] = new Vector3(x, 0, z);

        return verts;
    }

    private int[] GenerateTries()
    {
        //C#에서 사용되는 예외를 발생시키는 코드
        //개발자가 아직 해당 메서드를 완전히 구현하지 않았음을 나타내는 용도로 사용
        // throw new NotImplementedException(); 


        // 삼각형 2개 필요하다. 그러면 꼭짓점은 총 6개씩 필요하기 때분에 6개를 곱한다.
        var tries = new int[Mesh.vertices.Length * 6];

        // 2개의 삼각형이 같은 타일에 있다. for 문 사용
        for (int x = 0; x < Dimension; x++)
        {
            for (int z = 0; z < Dimension; z++)
            {
                tries[index(x, z) * 6 + 0] = index(x, z);
                tries[index(x, z) * 6 + 1] = index(x + 1, z + 1);
                tries[index(x, z) * 6 + 2] = index(x + 1, z);
                tries[index(x, z) * 6 + 3] = index(x, z);
                tries[index(x, z) * 6 + 4] = index(x, z + 1);
                tries[index(x, z) * 6 + 5] = index(x + 1, z + 1);
            }
        }

        return tries;

    }

    private Vector2[] GenerateUVs()
    {
        var uvs = new Vector2[Mesh.vertices.Length];

        //UV를 뒤집고 다시 설정하는 것보다 항상 n 타일 위에 하나의 UV를 설정한다.
        for (int x = 0; x <= Dimension; x++)
        {
            for (int z = 0; z <= Dimension; z++)
            {
                var vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }


    // x=0, z=0 이면 0 반환 , x=0,z=9이면 9 반환
    private int index(int x, int z)
    {
        return x * (Dimension + 1) + z;
    }

    private int index(float x, float z)
    {
        return index((int)x, (int)z);
    }


    // Update is called once per frame
    void Update()
    {
        var verts = Mesh.vertices;

        for (int x = 0; x <= Dimension; x++)
        {
            for (int z = 0; z <= Dimension; z++)
            {
                var y = 0f; // 정점 높이
                for (int o = 0; o < Octaves.Length; o++)
                {
                    if (Octaves[o].alternate) // 파도의 움직임을 본다면
                    {
                        // 높이에 따른 움직임을 나타낸다.
                        var perl = Mathf.PerlinNoise((x * Octaves[o].scale.x) / Dimension, (z * Octaves[o].scale.y) / Dimension) * Mathf.PI * 2f;
                        y += Mathf.Cos(perl + Octaves[o].speed.magnitude * Time.time) * Octaves[o].height;
                    }
                    else
                    {
                        // 0과 1사이 정규화하기 위해 0.5를 뺀다.
                        var perl = Mathf.PerlinNoise((x * Octaves[o].scale.x + Time.time * Octaves[o].speed.x) / Dimension, (z * Octaves[o].scale.y + Time.time * Octaves[o].speed.y) / Dimension) - 0.5f;
                        y += perl * Octaves[o].height;
                    }

                }
                verts[index(x, z)] = new Vector3(x, y, z);
            }
        }
        Mesh.vertices = verts;
        Mesh.RecalculateNormals();

  

    }

 


    /// <summary>
    /// 옥타브는 스케일 값 높이와 함께 피크 값을 가져야 한다.
    /// </summary>
    [Serializable]
    public struct Octave
    {
        [Tooltip("파도의 속도(주기)")]
        public Vector2 speed;
        [Tooltip("파도의 크기(메쉬의 균일함)")]
        public Vector2 scale;
        [Tooltip("파도의 높이")]
        public float height;
        [Tooltip("파도의 움직임 유/무")]
        public bool alternate;
    }
}
