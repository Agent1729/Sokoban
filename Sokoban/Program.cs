using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
	public enum tile
	{
		E, W, B, G, F, p, P, I
	}

	class Program
	{

		static void Main(string[] args)
		{
			int[,] mapints = new int[,]{{1,1,1,1,1,1},
										{1,3,0,0,3,1},
										{1,0,2,2,0,1},
										{1,3,2,5,1,1},
										{1,0,2,2,0,1},
										{1,3,0,0,3,1},
										{1,1,1,1,1,1}};
			Map map = new Map();
			map.setMap(mapints, 3, 3);

			map.print();

			do
			{
				ConsoleKeyInfo key = Console.ReadKey();
				if ((key.Key == ConsoleKey.W) || (key.Key == ConsoleKey.UpArrow))
				{
					map.key_up();
				}
				if ((key.Key == ConsoleKey.A) || (key.Key == ConsoleKey.LeftArrow))
				{
					map.key_left();
				}
				if ((key.Key == ConsoleKey.S) || (key.Key == ConsoleKey.DownArrow))
				{
					map.key_down();
				}
				if ((key.Key == ConsoleKey.D) || (key.Key == ConsoleKey.RightArrow))
				{
					map.key_right();
				}
				if (key.Key == ConsoleKey.Q)
				{
					break;
				}
				if(key.Key == ConsoleKey.R)
				{
					map.setMap(mapints, 3, 3);
				}

				Console.Clear();
				map.print();
			} while (true);
		}
	}

	public class Map
	{
		const int height = 7;
		const int width = 6;
		tile[,] map = new tile[height, width];
		int px = 0;
		int py = 0;

		public Map()
		{

		}

		public void setMap(int[,] mapInts, int _px, int _py)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					switch (mapInts[i, j])
					{
						case 0:
							map[i, j] = tile.E; break;
						case 1:
							map[i, j] = tile.W; break;
						case 2:
							map[i, j] = tile.B; break;
						case 3:
							map[i, j] = tile.G; break;
						case 4:
							map[i, j] = tile.F; break;
						case 5:
							map[i, j] = tile.p; break;
						case 6:
							map[i, j] = tile.P; break;
					}
				}
			}
			px = _px;
			py = _py;
		}

		public void print()
		{
			Console.WriteLine();
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					Console.Write(" ");
					switch (map[y, x])
					{
						case tile.E:
							Console.Write(" "); break;
						case tile.W:
							Console.Write("#"); break;
						case tile.B:
							Console.Write("O"); break;
						case tile.G:
							Console.Write("."); break;
						case tile.F:
							Console.Write("@"); break;
						case tile.p:
							Console.Write("p"); break;
						case tile.P:
							Console.Write("P"); break;
					}
				}
				Console.Write("\r\n");
			}
			Console.WriteLine();
		}

		public tile tileAt(int x, int y)
		{
			if ((x < 0) || (x >= width) || (y < 0) || (y >= height))
				return tile.I;
			return map[y, x];
		}

		public static bool isSolid(tile t)
		{
			switch (t)
			{
				//E, W, B, G, F, p, P, I
				case tile.E:
				case tile.G:
				case tile.p:
				case tile.P:
					return false;
				case tile.W:
				case tile.B:
				case tile.F:
				case tile.I:
					return true;
			}
			return true;
		}

		public static bool isBox(tile t)
		{
			switch (t)
			{
				//E, W, B, G, F, p, P, I
				case tile.B:
				case tile.F:
					return true;
				case tile.E:
				case tile.G:
				case tile.p:
				case tile.P:
				case tile.W:
				case tile.I:
					return false;
			}
			return false;
		}

		public void key_left()
		{
			int x = px; int y = py;
			int x2 = px - 1; int y2 = py;
			int x3 = px - 2; int y3 = py;

			doMove(x, y, x2, y2, x3, y3);
		}

		public void key_right()
		{
			int x = px; int y = py;
			int x2 = px + 1; int y2 = py;
			int x3 = px + 2; int y3 = py;

			doMove(x, y, x2, y2, x3, y3);
		}

		public void key_up()
		{
			int x = px; int y = py;
			int x2 = px; int y2 = py - 1;
			int x3 = px; int y3 = py - 2;

			doMove(x, y, x2, y2, x3, y3);
		}

		public void key_down()
		{
			int x = px; int y = py;
			int x2 = px; int y2 = py + 1;
			int x3 = px; int y3 = py + 2;

			doMove(x, y, x2, y2, x3, y3);
		}

		public void doMove(int x, int y, int x2, int y2, int x3, int y3)
		{
			tile curTile = tileAt(x, y);
			tile toTile = tileAt(x2, y2);
			tile pushToTile = tileAt(x3, y3);

			//If the player can move here
			if (!isSolid(toTile) ||
				(isBox(toTile) && !isSolid(pushToTile)))
			{
				if (curTile == tile.p)
				{
					map[y, x] = tile.E;
				}
				else if (curTile == tile.P)
				{
					map[y, x] = tile.G;
				}
				else
				{
					ERROR();
				}

				//If the player is pushing
				if (isBox(toTile))
				{
					//Move player into this spot
					if (toTile == tile.B)
					{
						map[y2, x2] = tile.p;
					}
					else if (toTile == tile.F)
					{
						map[y2, x2] = tile.P;
					}
					else
					{
						ERROR();
					}
					px = x2;
					py = y2;

					//Move box to next spot
					if (pushToTile == tile.E)
					{
						map[y3, x3] = tile.B;
					}
					else if (pushToTile == tile.G)
					{
						map[y3, x3] = tile.F;
					}
					else
					{
						ERROR();
					}
				}
				else //Not pushing
				{
					//Move player into this spot
					if (toTile == tile.E)
					{
						map[y2, x2] = tile.p;
					}
					else if (toTile == tile.G)
					{
						map[y2, x2] = tile.P;
					}
					else
					{
						ERROR();
					}
					px = x2;
					py = y2;
				}
			}
		}

		public void ERROR()
		{
			int i = 0;
			do
			{
				if(i % 10000 == 0)
				{
					Console.WriteLine("ERROR!");
				}
				i++;
			} while (true);
		}
	}
}
