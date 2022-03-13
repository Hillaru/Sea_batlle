using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursachTIMP
{
    public struct PosData
    {
        public int x, y;
        public bool vertical;
    }

    class AI
    {
        List<List<int>> PlayerMap = new List<List<int>>();
        Random rand = new Random();
        List<int> PlayerShips = new List<int>(), ActualPlayerShips = new List<int>();
        PosData FirstShipHit = new PosData(), LastAttack = new PosData();
        int FreeCells = 100;

        public AI()
        {
            PlayerShips.Add(4); ActualPlayerShips.Add(4);
            PlayerShips.Add(3); ActualPlayerShips.Add(3);
            PlayerShips.Add(3); ActualPlayerShips.Add(3);
            PlayerShips.Add(2); ActualPlayerShips.Add(2);
            PlayerShips.Add(2); ActualPlayerShips.Add(2);
            PlayerShips.Add(2); ActualPlayerShips.Add(2);
            PlayerShips.Add(1); ActualPlayerShips.Add(1);
            PlayerShips.Add(1); ActualPlayerShips.Add(1);
            PlayerShips.Add(1); ActualPlayerShips.Add(1);
            PlayerShips.Add(1); ActualPlayerShips.Add(1);

            FirstShipHit.x = 0;
            FirstShipHit.y = 0;

            for (int i = 0; i < 12; i++)
            {
                PlayerMap.Add(new List<int>());
                for (int j = 0; j < 12; j++)
                {
                    if (i > 0 && i < 11 && j > 0 && j < 11)
                        PlayerMap[i].Add(0);
                    else
                        PlayerMap[i].Add(-1);
                }
            }
        }

        public PosData GetShipPos(int length)
        {
            PosData result;
            result.vertical = rand.Next(0, 2) == 1;

            if (!result.vertical)
            {
                result.x = rand.Next(1, 11 - length);
                result.y = rand.Next(1, 10);
            }
            else
            {
                result.y = rand.Next(1, 11 - length);
                result.x = rand.Next(1, 10);
            }

            return result;
        }

        private PosData CheckSurroundings(int x, int y)
        {
            PosData result = new PosData();
            int[] Xgo = { 1, 0, -1, 0 };
            int[] Ygo = { 0, 1, 0, -1 };

            for (int i = 0; i < 4; i++)
                if (PlayerMap[y + Ygo[i]][x + Xgo[i]] == 0)
                {
                    result.y = y + Ygo[i];
                    result.x = x + Xgo[i];
                }
                else
                if (PlayerMap[y + Ygo[i]][x + Xgo[i]] > 0)
                {
                    result.y = y + Ygo[(i + 2) % 4];
                    result.x = x + Xgo[(i + 2) % 4];

                    if (PlayerMap[result.y][result.x] != 0)
                        throw new Exception("CantAttack");

                    return result;
                }

            return result;
        }

        public PosData GetAttackPos()
        {
            for (int ShipNum = 0; ShipNum < PlayerShips.Count; ShipNum++)
            {
                if (ActualPlayerShips[ShipNum] != PlayerShips[ShipNum] && ActualPlayerShips[ShipNum] != 0)
                {
                    FreeCells--;
                    if (PlayerMap[LastAttack.y][LastAttack.x] == -1)
                        LastAttack = CheckSurroundings(FirstShipHit.x, FirstShipHit.y);
                    else
                        try
                        {
                            LastAttack = CheckSurroundings(LastAttack.x, LastAttack.y);
                        }
                        catch
                        {                         
                            LastAttack = CheckSurroundings(FirstShipHit.x, FirstShipHit.y);
                        }

                    return LastAttack;
                }
            }

            int RandCell = rand.Next(1, FreeCells), CellI = 0;

            for (int y = 1; y < 11; y++)
                for (int x = 1; x < 11; x++)
                {
                    if (PlayerMap[y][x] == 0)
                        CellI++;

                    if (CellI == RandCell)
                    {
                        LastAttack.y = y;
                        LastAttack.x = x;
                        FreeCells--;
                        return LastAttack;
                    }                        
                }

            throw new Exception("CantAttack");
        }

        private void FillBadCells(int ShipNum, int y, int x)
        {
            int[] Xgo = { 1, 0, -1, 0, 1, -1, -1, 1};
            int[] Ygo = { 0, 1, 0, -1, 1, -1, 1, -1 };

            PlayerMap[y][x] = -1;

            for (int i = 0; i < 8; i++)
                if (PlayerMap[y + Ygo[i]][x + Xgo[i]] == 0)
                {
                    PlayerMap[y + Ygo[i]][x + Xgo[i]] = -1;
                    FreeCells--;
                }
                else
                if (PlayerMap[y + Ygo[i]][x + Xgo[i]] == ShipNum)
                    FillBadCells(ShipNum, y + Ygo[i], x + Xgo[i]);
        }

        public void SendAttackResults(int ResultCode)
        {
            if (ResultCode == -1)
                PlayerMap[LastAttack.y][LastAttack.x] = -1;
            else
            {
                PlayerMap[LastAttack.y][LastAttack.x] = ResultCode;
                ActualPlayerShips[ResultCode - 1]--;

                if (PlayerMap[FirstShipHit.y][FirstShipHit.x] != ResultCode)
                    FirstShipHit = LastAttack;

                if (ActualPlayerShips[ResultCode - 1] == 0)
                    FillBadCells(ResultCode, LastAttack.y, LastAttack.x);

            }
        }
    }
}
