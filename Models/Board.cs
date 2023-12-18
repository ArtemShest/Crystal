using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalClient.Models;

namespace CrystalClient.Models
{
    public class Board : IEnumerable<Cell>
    {
        private Cell[,] _area;



        public Player this[int row, int column]
        {
            get => _area[row, column].Parent;
            set => _area[row, column].Parent = value;
        }

        public Board() //конструктор поля
        {
            _area = new Cell[12, 12];                       //размеры поля
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 12; j++)
                {
                    _area[i, j] = new Cell(i, j);
                }
        }

        public void CheckDistance(Cell curCell)
        {
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 12; j++)
                {
                    _area[i, j].GetDistance(curCell);
                }
        }

        public void ClearDistance()
        {
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 12; j++)
                {
                    _area[i, j].DistanceFromCurCell = 0;
                }
        }

        public void GetArea(Player curPlayer)
        {
            int stepsCount = curPlayer.StepsLeft;
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 12; j++)
                {
                    if (_area[i, j].DistanceFromCurCell <= stepsCount)
                    {
                        if (_area[i, j].Parent == null)
                        {
                            _area[i, j].Color = _area[i, j].HighlighColor;
                            _area[i, j].CanMove = true;
                        }
                        else if (_area[i, j].Parent != curPlayer)
                        {
                            _area[i, j].Color = _area[i, j].HighlighColor;
                            _area[i, j].CanMove = true;
                        }
                    }
                }
        }

        public void ClearArea(Player curPlayer)
        {
            ClearDistance();
            for (int i = 0; i < 12; i++)
                for (int j = 0; j < 12; j++)
                {
                    if(_area[i,j].DistanceFromCurCell == 0)
                    {
                        if (_area[i, j].Parent == null)
                        {
                            _area[i, j].Color = _area[i, j].DefaultColor;
                            _area[i, j].CanMove = false;

                        }
                        else if (_area[i, j].Parent != curPlayer)
                        {
                            _area[i, j].Color = _area[i, j].Parent.Color;
                            _area[i, j].CanMove = false;
                        }
                    }
                }
        } 
    


        public IEnumerator<Cell> GetEnumerator()
            => _area.Cast<Cell>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _area.GetEnumerator();
    }
}
