using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursachTIMP
{
    public partial class SeaBattle : Form
    {
        HelpWindow help = new HelpWindow();
        List<List<int>> PlayerMap, AIMap;
        int PlayerX = 1, PlayerY = 1, AiY = 1, AiX = 1;
        List<int> PlayerShips, AiShips;
        int ShipNum = 1;
        AI ai;

        public SeaBattle()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
            var column = new DataGridViewColumn();
            var column2 = new DataGridViewColumn();

            for (int i = 0; i < 10; i++)
            {
                column = new DataGridViewColumn();
                column.Width = 26;
                column.ReadOnly = true;
                column.Name = i.ToString();
                column.Frozen = true;
                column.CellTemplate = new DataGridViewTextBoxCell();

                column2 = new DataGridViewColumn();
                column2.Width = 26;
                column2.ReadOnly = true;
                column2.Name = i.ToString();
                column2.Frozen = true;
                column2.CellTemplate = new DataGridViewTextBoxCell();

                dataGridView2.Columns.Add(column);
                dataGridView1.Columns.Add(column2);
            }

            dataGridView2.AllowUserToAddRows = false;
            dataGridView1.AllowUserToAddRows = false;

            NewGameStart();
        }

        void NewGameStart()
        {
            PlayerMap = new List<List<int>>();
            AIMap = new List<List<int>>();
            PlayerShips = new List<int>();
            AiShips = new List<int>();
            ShipNum = 1;

            setHor.Visible = true;
            setVert.Visible = true;
            setHor.Enabled = true;
            setVert.Enabled = true;
            label2.Visible = true;
            label1.Visible = true;
            attack.Visible = false;

            for (int i = 0; i < 16; i++)
            {
                PlayerMap.Add(new List<int>());
                AIMap.Add(new List<int>());

                for (int j = 0; j < 16; j++)
                {
                    PlayerMap[i].Add(0);
                    AIMap[i].Add(0);
                }

                if (i == 9)
                {
                    dataGridView1.DataSource = PlayerMap;
                    dataGridView2.DataSource = AIMap;
                }
            }

            dataGridView1.CurrentCell = null;
            dataGridView2.CurrentCell = null;

            PlayerShips.Add(4); AiShips.Add(4);
            PlayerShips.Add(3); AiShips.Add(3);
            PlayerShips.Add(3); AiShips.Add(3);
            PlayerShips.Add(2); AiShips.Add(2);
            PlayerShips.Add(2); AiShips.Add(2);
            PlayerShips.Add(2); AiShips.Add(2);
            PlayerShips.Add(1); AiShips.Add(1);
            PlayerShips.Add(1); AiShips.Add(1);
            PlayerShips.Add(1); AiShips.Add(1);
            PlayerShips.Add(1); AiShips.Add(1);

            ai = new AI();
            label2.Text = (PlayerShips[ShipNum - 1]).ToString();
            Status.Text = "Размещение\rкораблей\rигроком";
        }

        void UpdateMaps()
        {
            int temp;

            dataGridView1.CurrentCell = null;
            dataGridView2.CurrentCell = null;

            for (int i = 1; i < 11; i++)
                for (int j = 1; j < 11; j++)
                {
                    int row = i - 1, col = j - 1;
                    temp = PlayerMap[i][j];
                    switch (temp)
                    {
                        case 0:
                            dataGridView1.Rows[row].Cells[col].Value = "~";
                            break;
                        case -1:
                            dataGridView1.Rows[row].Cells[col].Value = "~";
                            dataGridView1.Rows[row].Cells[col].Style.BackColor = Color.Blue;
                            break;
                        case -2:
                            dataGridView1.Rows[row].Cells[col].Value = "X";
                            dataGridView1.Rows[row].Cells[col].Style.BackColor = Color.Orange;
                            break;
                        case -3:
                            dataGridView1.Rows[row].Cells[col].Value = "X";
                            dataGridView1.Rows[row].Cells[col].Style.BackColor = Color.Red;
                            break;
                        default:
                            dataGridView1.Rows[row].Cells[col].Value = "#";
                            dataGridView1.Rows[row].Cells[col].Style.BackColor = Color.GreenYellow;
                            break;
                    }

                    temp = AIMap[i][j];
                    switch (temp)
                    {
                        case -1:
                            dataGridView2.Rows[row].Cells[col].Value = "~";
                            dataGridView2.Rows[row].Cells[col].Style.BackColor = Color.Blue;
                            break;
                        case -2:
                            dataGridView2.Rows[row].Cells[col].Value = "X";
                            dataGridView2.Rows[row].Cells[col].Style.BackColor = Color.Orange;
                            break;
                        case -3:
                            dataGridView2.Rows[row].Cells[col].Value = "X";
                            dataGridView2.Rows[row].Cells[col].Style.BackColor = Color.Red;
                            break;
                        default:
                            dataGridView2.Rows[row].Cells[col].Value = "~";
                            break;
                    }

                }
        }

        bool FindFreeCell(List<List<int>> map, int x, int y)
        {
            if (x > 10 || y > 10)
                return false;

            if (map[y][x] != 0 || map[y][x + 1] != 0 || map[y][x - 1] != 0 || map[y + 1][x] != 0 || map[y - 1][x] != 0 ||
            map[y + 1][x + 1] != 0 || map[y - 1][x + 1] != 0 || map[y + 1][x - 1] != 0 || map[y - 1][x - 1] != 0)
                return false;

            return true;
        }

        public bool SetShip(ref List<List<int>> map, int x, int y, int length, bool vertical)
        {
            if (!vertical)
            {
                for (int i = 0; i < length; i++)
                    if (!FindFreeCell(map, x + i, y))
                        return false;

                for (int i = 0; i < length; i++)
                        map[y][x + i] = ShipNum;
            }   
            else
            {
                for (int i = 0; i < length; i++)
                    if (!FindFreeCell(map, x, y + i))
                        return false;

                for (int i = 0; i < length; i++)
                        map[y + i][x] = ShipNum;
            }

            return true;
        }

       public void SetShipEvent(bool vert)
        {
            setHor.Enabled = false;
            setVert.Enabled = false;

            if (dataGridView1.CurrentCell == null)
            {
                MessageBox.Show("Ошибка, ячейка для установки корабля не выбрана");
                setHor.Enabled = true;
                setVert.Enabled = true;
                return;
            }

            PlayerY = dataGridView1.CurrentCell.RowIndex + 1;
            PlayerX = dataGridView1.CurrentCell.ColumnIndex + 1;

            if (!SetShip(ref PlayerMap, PlayerX, PlayerY, PlayerShips[ShipNum - 1], vert))
            {
                MessageBox.Show("Ошибка, некорректное расположение корабля");
                setHor.Enabled = true;
                setVert.Enabled = true;
                return;
            }

            ShipNum++;
            if (ShipNum <= 10)
                label2.Text = PlayerShips[ShipNum - 1].ToString();
            UpdateMaps();

            if (ShipNum > 10)
            {
                setHor.Visible = false;
                setVert.Visible = false;
                label2.Visible = false;
                label1.Visible = false;
                ShipNum = 1;
                
                AISetShips();
                attack.Visible = true;

                Status.Text = "Игра началась\rВаш ход";
            }

            setHor.Enabled = true;
            setVert.Enabled = true;
        }

        private void setHor_Click(object sender, EventArgs e)
        {
            SetShipEvent(false);
        }

        private void setVert_Click(object sender, EventArgs e)
        {
            SetShipEvent(true);
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help.Show();
        }

        private void AISetShips()
        {          
            int length;
            PosData data;

            while (ShipNum < 11)
            {
                length = AiShips[ShipNum - 1];
                data = ai.GetShipPos(length);

                if (SetShip(ref AIMap, data.x, data.y, length, data.vertical))
                    ShipNum++;
            }
        }

        private void новаяИграToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(
                        "Вы действительно хотите начать новую игру?",
                        "Новая игра",
                        MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
                NewGameStart();
        }

        async private void attack_Click(object sender, EventArgs e)
        {
            attack.Enabled = false;
            int AtkResult = 1;
            int sum = 0;

            AiY = dataGridView2.CurrentCell.RowIndex + 1;
            AiX = dataGridView2.CurrentCell.ColumnIndex + 1;

            AtkResult = Attack(ref AIMap, ref AiShips, AiX, AiY);

            switch (AtkResult)
            {
                case -1:
                    MessageBox.Show("Ошибка, вы уже атаковали эту область");
                    attack.Enabled = true;
                    return;
                case 0:
                    Status.Text = "Промах!";
                    break;
                case 1:
                    Status.Text = "Попадание!";
                    break;
                case 2:
                    Status.Text = "Уничтожил!";
                    break;
            }

            UpdateMaps();

            sum = 0;
            foreach (int hp in AiShips)
                sum += hp;

            if (sum == 0)
            {
                Status.Text = "ПОБЕДА!";
                attack.Visible = false;
                return;
            }

            await Task.Delay(700);

            if (AtkResult > 0)
            {
                UpdateMaps();
                attack.Enabled = true;
                Status.Text = "Ваш ход";
                return;
            }

            Status.Text = "Противник\rходит";

            AtkResult = 1;
            PosData data;

            while (AtkResult != 0)
            {
                data = ai.GetAttackPos();
                AtkResult = Attack(ref PlayerMap, ref PlayerShips, data.x, data.y, true);

                sum = 0;
                foreach (int hp in PlayerShips)
                    sum += hp;

                if (sum == 0)
                {
                    UpdateMaps();
                    Status.Text = "ПОРАЖЕНИЕ!";
                    attack.Visible = false;
                    return;
                }

                UpdateMaps();
                await Task.Delay(1000);
            }

            attack.Enabled = true;
            Status.Text = "Ваш ход";
        }

        private void FillBadCells(ref List<List<int>> map, int y, int x)
        {
            int[] Xgo = { 1, 0, -1, 0, 1, -1, -1, 1 };
            int[] Ygo = { 0, 1, 0, -1, 1, -1, 1, -1 };

            map[y][x] = -3;

            for (int i = 0; i < 8; i++)
                if (map[y + Ygo[i]][x + Xgo[i]] == 0)
                {
                    map[y + Ygo[i]][x + Xgo[i]] = -1;              
                }
                else
                if (map[y + Ygo[i]][x + Xgo[i]] == -2)
                    FillBadCells(ref map, y + Ygo[i], x + Xgo[i]);
        }

        private int Attack(ref List<List<int>> map, ref List<int> shiplist, int x, int y, bool send = false)
        {
            int temp = map[y][x];

            switch (temp)
            {
                case 0:                   
                    map[y][x] = -1;
                    if (send)
                        ai.SendAttackResults(-1);
                    return 0;

                case -1:
                    return -1;

                case -2:
                    return -1;

                default:
                    shiplist[temp - 1]--;
                    map[y][x] = -2;

                    if (send)
                        ai.SendAttackResults(temp);

                    if (shiplist[temp - 1] == 0)
                    {                           
                        FillBadCells(ref map, y, x);
                        return 2;
                    }                                               
                    else
                        return 1;                                            
            }                                  
        }
    }
}
