using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace CrystalClient.Models
{
    static class SystemSettings
    {
        private static int bufCountPlayers = 4;
        private static int countPlayers; //кол-во игроков

        private static int bufPlayersSteps = 4;
        private static int stepsOnMotion; //кол-во шагов игрока за один ход

        public static int CountActivePlayers;


        public static int BufPlayersSteps
        {
            get => bufPlayersSteps;
            set => bufPlayersSteps = value;
        }
        public static int BufCountPlayers
        {
            get => bufCountPlayers;
            set => bufCountPlayers = value;
        }

        //        private static int LenghtOfArea; //размер поля

        public static int CountPlayers
        {
            get => countPlayers;
            set => countPlayers = value;
        }

        public static int StepsOnMotion
        {
            get => stepsOnMotion;
            set => stepsOnMotion = value;
        }

    }
}
