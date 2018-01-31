using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random; 		//Tells Random to use the Unity Engine random number generator.

namespace Completed
	
{
	
	public class BoardManager : MonoBehaviour
	{		
        // マップの数
        [SerializeField] private int columns = 30;
        [SerializeField] private int rows = 30;
        // 壁の数最小値最大値
		[SerializeField] private Vector2 wallCount = new Vector2 (5, 9);
        // 出口のオブジェクト
        [SerializeField] private GameObject exit;
        // フロアで出すタイル
        [SerializeField] private GameObject[] floorTiles;
        // 壊れる壁のタイル
        [SerializeField] private GameObject[] wallTiles;
        // 敵のプレハブ 
        [SerializeField] private GameObject[] enemyTiles;
        // 外の壁タイル
        [SerializeField] private GameObject[] outerWallTiles;

        private Transform boardHolder;								
		private List <Vector3> gridPositions = new List <Vector3> ();
		
        // 座標リスト初期化
		void InitialiseList ()
		{
			gridPositions.Clear ();
			
			for(int x = 0; x < columns; x++){
				for(int y = 0; y < rows; y++){
                    gridPositions.Add(new Vector3(x, y, 0.0f));
				}
			}
		}
		
        // ボーダーのセットアップ		
		void BoardSetup ()
		{
            // 親オブジェクト作成
			boardHolder = new GameObject ("Board").transform;
			
            // 配置
			for(int x = 0; x < columns; x++)
			{
				for(int y = 0; y < rows; y++)
				{
                    // ランダムなフロアタイルを参照
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];

                    // 端は端の壁を置くため参照変更
                    if ((x == 0) || x == columns - 1 || (y == 0) || y == rows - 1)
                        toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    // オブジェクト作成
                    GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0.0f), Quaternion.identity) as GameObject;
					
                    // 作ったオブジェクトを親に追加
					instance.transform.SetParent (boardHolder);
				}
			}
		}
		
		Vector3 RandomPosition ()
		{
            // ランダムな座標インデックス取得
			int randomIndex = Random.Range (1, gridPositions.Count-1);
			
            // インデックスの座標取得
			Vector3 randomPosition = gridPositions[randomIndex];
			
            // 既にあるタイルを削除
			gridPositions.RemoveAt (randomIndex);
			
            // 座標を返す
			return randomPosition;
		}
				
		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
            // オブジェクトの数を設定
			int objectCount = Random.Range (minimum, maximum+1);
			
			for(int i = 0; i < objectCount; i++)
			{
                // ランダムな座標取得
				Vector3 randomPosition = RandomPosition();
				
                // 座標設定しなおし
                while(randomPosition == new Vector3(columns - 2, rows - 2, 0f)){
                    randomPosition = RandomPosition();
                }

                // タイルを取得
                GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

                // 作成
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
            }
        }
		
		public void SetupScene (int level)
		{
            // ボードの設定
			BoardSetup ();
			
            // リスト初期化
			InitialiseList ();

            // 壁タイル作成。
            LayoutObjectAtRandom (wallTiles, (int)wallCount.x, (int)wallCount.y);

            // 敵の数を設定
            int enemyCount = (int)Mathf.Log(level, 2f);
			
            // 敵の作成
			LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);

			// 出口の作成
			Instantiate (exit, new Vector3 (columns - 2, rows - 2, 0f), Quaternion.identity);
		}
	}
}
