using UnityEngine;

namespace AnyRPG {

    public class TerrainDetector {
        private TerrainData terrainData;
        private int alphamapWidth;
        private int alphamapHeight;
        private float[,,] splatmapData;
        private int numTextures;

        public void LoadSceneSettings() {
            //Debug.Log("TerrainDetector.LoadSceneSettings()");
            if (Terrain.activeTerrain == null) {
                ClearSceneSettings();
                return;
            }
            terrainData = Terrain.activeTerrain.terrainData;
            alphamapWidth = terrainData.alphamapWidth;
            alphamapHeight = terrainData.alphamapHeight;

            splatmapData = terrainData.GetAlphamaps(0, 0, alphamapWidth, alphamapHeight);
            numTextures = splatmapData.Length / (alphamapWidth * alphamapHeight);
        }

        public void ClearSceneSettings() {
            terrainData = null;
            alphamapWidth = 0;
            alphamapHeight = 0;
            splatmapData = new float[,,] { };
            numTextures = 0;
        }

        // Новый метод: находит террейн, на котором стоит игрок
        private Terrain GetTerrainAtPosition(Vector3 worldPosition) {
            Terrain[] terrains = Terrain.activeTerrains;
            foreach (Terrain ter in terrains) {
                Vector3 terPos = ter.transform.position;
                Vector3 terSize = ter.terrainData.size;
                // Проверяем, попадает ли позиция в границы террейна (по X и Z)
                if (worldPosition.x >= terPos.x && worldPosition.x <= terPos.x + terSize.x &&
                    worldPosition.z >= terPos.z && worldPosition.z <= terPos.z + terSize.z) {
                    return ter;
                }
            }
            return null;
        }

        private Vector3 ConvertToSplatMapCoordinate(Vector3 worldPosition, Terrain ter) {
            Vector3 splatPosition = new Vector3();
            Vector3 terPosition = ter.transform.position;
            splatPosition.x = ((worldPosition.x - terPosition.x) / ter.terrainData.size.x) * ter.terrainData.alphamapWidth;
            splatPosition.z = ((worldPosition.z - terPosition.z) / ter.terrainData.size.z) * ter.terrainData.alphamapHeight;
            return splatPosition;
        }

        public int GetActiveTerrainTextureIdx(Vector3 position) {
            // Находим террейн под игроком
            Terrain ter = GetTerrainAtPosition(position);
            if (ter == null) {
                // Игрок вне террейнов — возвращаем 0 (безопасное значение)
                return 0;
            }

            TerrainData data = ter.terrainData;
            int width = data.alphamapWidth;
            int height = data.alphamapHeight;
            int texCount = data.alphamapLayers;

            Vector3 terrainCord = ConvertToSplatMapCoordinate(position, ter);

            // Защита от выхода за границы массива
            int x = Mathf.Clamp((int)terrainCord.x, 0, width - 1);
            int z = Mathf.Clamp((int)terrainCord.z, 0, height - 1);

            // Если данные для этого террейна ещё не загружены — загружаем
            if (splatmapData == null || splatmapData.GetLength(0) != height || splatmapData.GetLength(1) != width) {
                splatmapData = data.GetAlphamaps(0, 0, width, height);
                numTextures = texCount;
            }

            int activeTerrainIndex = 0;
            float largestOpacity = 0f;

            for (int i = 0; i < numTextures; i++) {
                if (largestOpacity < splatmapData[z, x, i]) {
                    activeTerrainIndex = i;
                    largestOpacity = splatmapData[z, x, i];
                }
            }

            return activeTerrainIndex;
        }
    }
}