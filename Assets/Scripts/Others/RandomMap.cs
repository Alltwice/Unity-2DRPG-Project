using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;  // Tilemap 组件
    public RuleTile groundTile;  // 地面瓦片
    public RuleTile waterTile;    // 墙壁瓦片
    public int mapWidth = 50; // 地图宽度
    public int mapHeight = 50; // 地图高度
    public float noiseScale = 0.1f; // 噪声尺度
    public float threshold = 0.5f; // 噪声阈值

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        // 生成地图中的每一个瓦片
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // 获取每个位置的柏林噪声值
                float noiseValue = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);

                // 根据噪声值决定瓦片类型
                RuleTile tileToPlace = noiseValue > threshold ? groundTile : waterTile;

                // 在 Tilemap 中设置相应位置的瓦片
                tilemap.SetTile(new Vector3Int(x, y, 0), tileToPlace);
            }
        }
    }
}

