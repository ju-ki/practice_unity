using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTerrainField : MonoBehaviour
{
    public int width = 50;
    public int height = 30;
    public float scale = 5.0f;

    const float TriangleHeight = 0.86645f;
    const float TriangleHeightDouble = 1.7232f;

    Mesh mesh;

    [ContextMenu("生成")]
    private void makeGround()
    {
        mesh = new Mesh();

        int p;

        mesh.Clear();

        var vertices = new Vector3[((width + 1) * 2 + 1) * (height + 1) + width + 1];
        var triangles = new int[(width * 2 + 1) * (height * 2 + 2) * 3];

        for (p = 0; p <= width; p++)
        {
            vertices[p].x = p * scale;
            vertices[p].z = 0f;
            vertices[p].y = 0f;
        }
        for (int i = 0; i <= height; i++)
        {
            // 左端処理
            vertices[p].x = 0f;
            vertices[p].z = i * TriangleHeightDouble * scale + TriangleHeight * scale;
            vertices[p].y = 0f;
            p++;

            for (int j = 0; (j <= width - 1); j++)
            {
                vertices[p].x = j * scale + 0.5f * scale;
                vertices[p].z = i * TriangleHeightDouble * scale + TriangleHeight * scale;
                vertices[p].y = 0f;
                p++;
            }

            // 右端処理
            vertices[p].x = width * scale;
            vertices[p].z = i * TriangleHeightDouble * scale + TriangleHeight * scale;
            vertices[p].y = 0f;
            p++;

            // 縦列はワンループで2つの三角形がペアです。
            for (int j = 0; j <= width; j++)
            {
                vertices[p].x = j * scale;
                vertices[p].z = (i + 1f) * TriangleHeightDouble * scale;
                vertices[p].y = 0f;
                p++;
            }
        }

        p = 0;
        // メッシュ順作成
        for (int i = 0; i <= height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // 三角形4個を一組にして定義していきます。
                triangles[p + 0] = j + (((width + 1) * 2 + 1) * i);
                triangles[p + 1] = j + (width + 1) * (i * 2 + 1) + i;
                triangles[p + 2] = j + (width + 1) * (i * 2 + 1) + i + 1;

                triangles[p + 3] = j + (((width + 1) * 2 + 1) * i);  // 
                triangles[p + 4] = j + (width + 1) * (i * 2 + 1) + i + 1;
                triangles[p + 5] = j + (((width + 1) * 2 + 1) * i) + 1;

                triangles[p + 6] = j + (width + 1) * (i * 2 + 1) + i;
                triangles[p + 7] = j + (((width + 1) * 2 + 1) * (i + 1));
                triangles[p + 8] = j + (width + 1) * (i * 2 + 1) + i + 1;

                triangles[p + 9] = j + (width + 1) * (i * 2 + 1) + i + 1;
                triangles[p + 10] = j + (((width + 1) * 2 + 1) * (i + 1));
                triangles[p + 11] = j + (((width + 1) * 2 + 1) * (i + 1)) + 1;

                p += 12;
            }
            // 右恥と、左端処理
            triangles[p + 0] = width + (((width + 1) * 2 + 1) * i);
            triangles[p + 1] = width + (width + 1) * (i * 2 + 1) + i;
            triangles[p + 2] = width + (width + 1) * (i * 2 + 1) + i + 1;

            triangles[p + 3] = width + (width + 1) * (i * 2 + 1) + i;
            triangles[p + 4] = width + (((width + 1) * 2 + 1) * (i + 1));
            triangles[p + 5] = width + (width + 1) * (i * 2 + 1) + i + 1;

            p += 6;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        var filter = GetComponent<MeshFilter>();
        filter.sharedMesh = mesh;
    }
}
