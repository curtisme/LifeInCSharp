using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class GOL
{
    private class Board
    {
        private int[][] cells;
        private int width, height;

        public Board(int x, int y)
        {
            width = x;
            height = y;
            cells = new int[y][];
            for (int i=0;i<y;i++)
            {
                cells[i] = new int[x];
                for (int j=0;j<x;j++)
                {
                    cells[i][j] = 0;
                }
            }

        }

        public void RandomInit(double density)
        {
            if (density > 1)
                throw new Exception($"{density} is greater than 1!");
            Random r = new Random();
            for (int i=0;i<cells.Length;i++)
            {
                for (int j=0;j<cells[i].Length;j++)
                    cells[i][j] = r.NextDouble() < density ? 1 : 0;
            }
        }

        public void NextState()
        {
            int[][] next = new int[height][];
            for (int j=0;j<height;j++)
            {
                next[j] = new int[width];
                for (int i=0;i<width;i++)
                {
                    int[] nbhd = GetNbhd(i, j);
                    int count = nbhd.Sum();
                    switch (nbhd[4])
                    {
                        case 0:
                            next[j][i] = count == 3 ? 1 : 0;
                            break;
                        default:
                            count -= 1;
                            next[j][i] = count == 2 || count == 3 ? 1 : 0;
                            break;
                    }
                }
            }
            cells = next;
        }

        public string NbhdString(int i, int j)
        {
            int[] nbhd = GetNbhd(i,j);
            StringBuilder sb = new StringBuilder();
            foreach (int cell in nbhd)
            {
                sb.Append(cell < 1 ? '_' : '*');
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int[] row in cells)
            {
                foreach (int cell in row)
                {
                    sb.Append(cell == 0 ? "  " : "* ");
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        private int PositiveMod(int n, int m)
        {
            int ret = n % m;
            return ret < 0 ? m + ret : ret;
        }

        private int[] GetNbhd(int i, int j)
        {
            return new int[] {
                cells[PositiveMod(j - 1, height)][PositiveMod(i - 1, width)],
                cells[PositiveMod(j - 1, height)][PositiveMod(i, width)],
                cells[PositiveMod(j - 1, height)][PositiveMod(i + 1, width)],
                cells[PositiveMod(j, height)][PositiveMod(i - 1, width)],
                cells[PositiveMod(j, height)][PositiveMod(i, width)],
                cells[PositiveMod(j, height)][PositiveMod(i + 1, width)],
                cells[PositiveMod(j + 1, height)][PositiveMod(i - 1, width)],
                cells[PositiveMod(j + 1, height)][PositiveMod(i, width)],
                cells[PositiveMod(j + 1, height)][PositiveMod(i + 1, width)]
            };
        }
    }

    public static void Main(string[] args)
    {
        int x,y;
        switch (args.Length)
        {
            case 0:
                x = y = 10;
                break;
            case 1:
                if (!int.TryParse(args[0], out x))
                    goto case 0;
                y = x;
                break;
            default:
                if (!int.TryParse(args[0], out x) || !int.TryParse(args[1], out y))
                    goto case 0;
                break;

        }
        Board b = new Board(x, y);
        b.RandomInit(0.25);
        Task t = Task.Run(() => {
                while(true)
                {
                Console.Clear();
                Console.WriteLine(b);
                b.NextState();
                Thread.Sleep(250);
                }
                });
        Console.ReadKey();
        Console.Clear();
    }
}
