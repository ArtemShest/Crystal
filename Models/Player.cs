using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClient.Models
{
    public class Player : ReactiveObject
    {
        public int Id { get; set; }
        //        public string Name { get; set; } //имя игрока

        public int MaxSteps; //Всего шагов на ход

        private int stepsLeft; //Шагов осталось сделать
        public int StepsLeft
        {
            get => stepsLeft;
            set => this.RaiseAndSetIfChanged(ref stepsLeft, value);
        }

        private int countThings = 4; //сколько фишек осталось в распоряжении игрока
        public int CountThings
        {
            get => countThings;
            set
            {
                if (value >= 0)
                    this.RaiseAndSetIfChanged(ref countThings, value);
            }
        }


        public string HighlighColor;

        private string color;
        public string Color
        {
            get => color;
            set => this.RaiseAndSetIfChanged(ref color, value);
        }

        public bool Active { get; set; }

        public Player(string color, string defColor, bool active)
        {
            HighlighColor = defColor;
            Color = color;
            Active = active;

         }
    }
}
