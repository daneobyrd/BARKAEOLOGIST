using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainColor {
    Black,
    Red,
    Green,
    Blue
}

[System.Serializable]
public class AudioTerrainMap {
    public AudioList blackTerrainList;
    public AudioList redTerrainList;
    public AudioList greenTerrainList;
    public AudioList blueTerrainList;

    private static (Vector3, TerrainColor)[] _map = {
        (new Vector3(0f, 0f, 0f), TerrainColor.Black),
        (new Vector3(1f, 0f, 0f), TerrainColor.Red),
        (new Vector3(0f, 1f, 0f), TerrainColor.Green),
        (new Vector3(0f, 0f, 1f), TerrainColor.Blue),
    };

    public static TerrainColor GetTerrainColor(Color color) {
        Vector3 rgb = (Vector4) color;
        float best = Mathf.Infinity;
        TerrainColor bestColor = TerrainColor.Black;
        for (int i = 0; i < _map.Length; i++) {
            float d2 = (rgb -
                        _map[i]
                            .Item1).sqrMagnitude;
            if (d2 < best) {
                best = d2;
                bestColor = _map[i]
                    .Item2;
            }
        }

        return bestColor;
    }

    public AudioClip GetMatchingClip(TerrainColor color) {
        AudioList list = blackTerrainList;
        switch (color) {
            case TerrainColor.Black:
                list = blackTerrainList;
                break;
            case TerrainColor.Red:
                list = redTerrainList;
                break;
            case TerrainColor.Green:
                list = greenTerrainList;
                break;
            case TerrainColor.Blue:
                list = blueTerrainList;
                break;
        }

        return list.GetRandomClip();
    }
}
