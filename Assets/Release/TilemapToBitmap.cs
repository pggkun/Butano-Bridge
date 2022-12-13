using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Drawing;
using System.Drawing.Imaging;
using FreeImageAPI;
using System.Linq;

public class TilemapToBitmap : MonoBehaviour
{
    private ButanoManager manager;
    private int minX, minY, maxX, maxY;
    private float spriteWidth, spriteHeight;
    private Tilemap tilemap;
    private Texture2D resultTexture;
    private string resultFilename;

    public void Adjust8x8Tiles()
    {
        Grid grid = transform.parent.GetComponent<Grid>(); 
        grid.cellSize = new Vector3(0.08f, 0.08f, 0);
    }

    public void Adjust16x16Tiles()
    {
        Grid grid = transform.parent.GetComponent<Grid>(); 
        grid.cellSize = new Vector3(0.16f, 0.16f, 0);
    }

    public void ExportTilemap()
    {
        FindManager();
        FindEdges();
        GetTextures();
        ExportPng();
        ConvertToBitmap();
        GenerateJson();
    }

    private void FindManager()
    {
        GameObject[] sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>() ;    
        foreach(var so in sceneObjects)
        {
            if(so.GetComponent<ButanoManager>() != null)
            {
                manager = so.GetComponent<ButanoManager>();
            }
        }
    } 

    private void FindEdges()
    {
        tilemap = GetComponent<Tilemap>();
        Vector3Int pos;
        for (int x = 0; x < 5000; x++)
        {
            for (int y = 0; y < 5000; y++)
            {
                pos = new Vector3Int(-x, -y, 0);
                if (tilemap.GetSprite(pos) != null)
                {
                    Sprite sprite = tilemap.GetSprite(pos);
                    spriteWidth = sprite.rect.width;
                    spriteHeight = sprite.rect.height;
                    if (minX > pos.x)
                    {
                        minX = pos.x;
                    }
                    if (minY > pos.y)
                    {
                        minY = pos.y;
                    }
                }

                pos = new Vector3Int(x, y, 0);
                if (tilemap.GetSprite(pos) != null)
                {
                    if (maxX < pos.x)
                    {
                        maxX = pos.x;
                    }
                    if (maxY < pos.y)
                    {
                        maxY = pos.y;
                    }
                }
            }
        }
    }

    Texture2D GetCurrentSprite(Sprite sprite)
    {
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                         (int)sprite.textureRect.y,
                                         (int)sprite.textureRect.width,
                                         (int)sprite.textureRect.height);

        Texture2D texture = new Texture2D((int)sprite.textureRect.width,
                                         (int)sprite.textureRect.height);

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private void GetTextures()
    {
        Texture2D image = new Texture2D((int)spriteWidth * tilemap.size.x, (int)spriteHeight * tilemap.size.y);

        UnityEngine.Color[] transp = new UnityEngine.Color[image.width * image.height];
        for (int i = 0; i < transp.Length; i++)
        {
            transp[i] = new UnityEngine.Color(0f, 0f, 0f, 0f);
        }
        image.SetPixels(0,0,image.width, image.height, transp);

        for (int x = minX; x <= maxX; x++)
        {
            for(int y = minY; y <= maxY; y++)
            {
                if (tilemap.GetSprite(new Vector3Int(x, y, 0)) != null)
                {
                    image.SetPixels((x - minX) * (int)spriteWidth, (y - minY) * (int)spriteHeight, 
                                    (int)spriteWidth, (int)spriteHeight, 
                                    GetCurrentSprite(tilemap.GetSprite(new Vector3Int(x, y, 0))).GetPixels());
                }
            }
        }
        image.Apply();
        resultTexture = image;
    }

    private void ExportPng ()
    {
        byte[] bytes = resultTexture.EncodeToPNG();
        var dirPath = manager.Settings.ProjectGFXFolder;
        if (!Directory.Exists(dirPath))
        {
            UnityEngine.Debug.Log("Project GFX folder does not exists, please set a valid folder");
        }
        File.WriteAllBytes(Path.Combine(dirPath, tilemap.gameObject.name + ".png"), bytes);
    }

    private void ConvertToBitmap()
    {
        FIBITMAP dib = FreeImage.LoadEx(Path.Combine(manager.Settings.ProjectGFXFolder, tilemap.gameObject.name + ".png"));
        FIBITMAP clone = FreeImage.ConvertColorDepth(dib, FREE_IMAGE_COLOR_DEPTH.FICD_08_BPP, true);
        Bitmap bitmap = FreeImage.GetBitmap(clone);
        string origName = tilemap.gameObject.name.Replace("(", "").Replace(")","").Replace(" ", "");
        string bmpName = string.Concat(
                         origName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString().ToLower() : x.ToString().ToLower())
                     ); 
        bitmap.Save(Path.Combine(manager.Settings.ProjectGFXFolder, bmpName + ".bmp"), ImageFormat.Bmp);
        File.Delete(Path.Combine(manager.Settings.ProjectGFXFolder, tilemap.gameObject.name + ".png"));
    }

    private void GenerateJson()
    {
        string content = "{\"type\": \"regular_bg\"}";
        string origName = tilemap.gameObject.name.Replace("(", "").Replace(")","").Replace(" ", "");
        string resName = string.Concat(
                         origName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString().ToLower() : x.ToString().ToLower())
                     ); 
        string jsonName = Path.Combine(manager.Settings.ProjectGFXFolder, resName + ".json");
        using (StreamWriter sw = File.CreateText(jsonName))
        {
            sw.WriteLine(content);
        }	
    }    
}