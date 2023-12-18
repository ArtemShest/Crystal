using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalClient.Models;
using ReactiveUI;

namespace CrystalClient.Models
{
    public class Cell : ReactiveObject
    {
        private string _color;
        private string defaultColor = CellColor.White;
        private string highlighColor; //цвет выделения ячейки
        private bool canMove;
        public bool CanMove
        {
            get => canMove;
            set => canMove = value;
        }
        public string DefaultColor
        {
            get => defaultColor;
            set => defaultColor = value;
        }
        public string HighlighColor
        {
            get => highlighColor;
            set => highlighColor = value;
        }


        private int distanceFromCurCell;
        public int DistanceFromCurCell 
        {
            get => distanceFromCurCell;
            set => this.RaiseAndSetIfChanged(ref distanceFromCurCell, value);
        }

        public void GetDistance(Cell curCell) //расстояние от выбранной ячейки до этой
        {
            DistanceFromCurCell = (Math.Abs(X - curCell.X) + Math.Abs(Y - curCell.Y));
        }


        private bool _active;
        public int[] Coord = new int[2];

        private Player? parent;
        public Player? Parent
        {
            get => parent;
            set
            {
                if (value != null)
                {
                    Color = value.Color;
                    HighlighColor = value.HighlighColor;
                }
                else
                {
                    Color = CellColor.White;
                    HighlighColor = CellColor.LightBlue;
                }
                this.RaiseAndSetIfChanged(ref parent, value);
            }
        }

        private int _x;
        private int _y;

        public int X
        {
            get => _x;
            set
            {
                this.RaiseAndSetIfChanged(ref _x, value);
            }
        }
        public int Y
        {
            get => _y;
            set
            {
                this.RaiseAndSetIfChanged(ref _y, value);
            }
        }


        public Cell(int x, int y, Player player = null)
        {
            Parent = player;
            
            X = x;
            Y = y;
        }

        public string Color
        {
            get => _color;
            set
            {
                this.RaiseAndSetIfChanged(ref _color, value);
            }
        }
        public bool Active
        {
            get => _active;
            set
            {
                this.RaiseAndSetIfChanged(ref _active, value);
            }
        }
    }
}
