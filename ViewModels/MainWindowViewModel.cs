using CrystalClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ReactiveUI;
using System.Reactive;

namespace CrystalClient.ViewModels
{
    public enum State
    {
        NoInit,
        Ready,
        Going,
        Over
    }
    public class MainWindowViewModel : ViewModelBase
    {
        public State GameState = State.NoInit;
        

        private bool isGameOver = false;
        public bool IsGameOver
        {
            get { return isGameOver; }
            set { this.RaiseAndSetIfChanged(ref isGameOver, value); }
        }

        public Cell CurCell;
        private Board _board = new Board();

        public List<Player> PlayersList = new List<Player>();

        public string playerName = "Player";
        public string PlayerName
        {
            get { return playerName; }
            set { this.RaiseAndSetIfChanged(ref playerName, value); } 
        }

        Player RedPlayer;
        Player BluePlayer;
        Player GreenPlayer;
        Player YellowPlayer;

        private Player curPlayer;
        public Player CurPlayer
        {
            get => curPlayer;
            set => this.RaiseAndSetIfChanged(ref curPlayer, value);
        }

        public ReactiveCommand<Cell, Unit> DoCellCommand { get; }

        public MainWindowViewModel()
        {
            DoCellCommand = ReactiveCommand.Create<Cell>(RunCellCommand);
            
        }


        void RunCellCommand(Cell cell)
        {
            if ((!IsGameOver) && (GameState != State.NoInit )) //если игра не завершена
            {
                if (CurCell != null) //если ячейка уже выбранная
                {
                    if (cell == CurCell) //если клик по выбранной ячейке
                    {
                        Board.ClearDistance();
                        Board.ClearArea(CurPlayer);       // очищаем пути //ход завершен
                        CurCell = null;             //убираем выбранную ячейку
                    }
                    else if (cell.Parent == CurPlayer) //клик по другой ячейке того же цвета
                    {
                        //Board.CheckDistance(cell);
                        Board.ClearArea(CurPlayer);       // очищаем пути //ход завершен
                        CurCell = cell;             //новая выбранная ячейка
                        Board.CheckDistance(cell);
                        Board.GetArea(CurPlayer);          //рисуем новые возможные ходы
                    }

                    else if ((cell.Parent == null) && (cell.CanMove == true)) //если клик по клетке возможного хода
                    {
                        cell.Parent = CurPlayer;
                        CurPlayer.StepsLeft -= cell.DistanceFromCurCell;
                        CurCell.Parent = null;
                        CurCell = null;
                        Board.ClearArea(CurPlayer);       // очищаем пути //ход завершен

                        if (CurPlayer.StepsLeft == 0)
                            NextPlayer(CurPlayer);      //передаем ход следущему игроку
                    }

                    else if ((cell.Parent == null) && (cell.CanMove == false))  //если клик по пустому полю
                    {
                        Board.ClearDistance();
                        Board.ClearArea(CurPlayer);       // очищаем пути //ход завершен
                        CurCell = null;             //убираем выбранную ячейку
                                                    //                    return; //клик по белому полю
                    }

                    else if ((cell.Parent != null) && (cell.CanMove == true)) //если клик по ячейке другого игрока ///?????
                    {
                        cell.Parent.CountThings -= 1;
                        if (cell.Parent.CountThings == 0)
                            cell.Parent.Active = false;
                        CurPlayer.StepsLeft -= cell.DistanceFromCurCell;
                        ///если игроков больше не осталось - завершение игры

                        Board.ClearArea(CurPlayer);       // очищаем пути //ход завершен
                        cell.Parent = CurPlayer;
                        CurCell.Parent = null;

                        /// //проверка остался ли игрок один
                        {
                            int activePlayers = 0;
                            foreach (Player player in PlayersList)
                                if (player.Active) activePlayers++;

                            if ((activePlayers == 0) || (activePlayers == 1)) // победа текущего игрока
                            {
                                IsGameOver = true;
                                //открыть модальное окно
                            }
                        }

                        if (CurPlayer.StepsLeft == 0)
                            NextPlayer(CurPlayer);      //передаем ход следущему игроку
                    }

                }
                else if (CurCell == null)
                {
                    if (cell.Parent == CurPlayer)
                    {
                        CurCell = cell;
                        Board.CheckDistance(cell);
                        //Board.GetArea(SystemSettings.StepsOnMotion, CurPlayer);

                        Board.GetArea(CurPlayer);
                    }
                }
            }
        }

        public void SetPlayersSteps(int playersSteps)
        {
            SystemSettings.BufPlayersSteps = playersSteps;
        }

        public void SetPlayersCount(int playersCount)
        {
            SystemSettings.BufCountPlayers = playersCount;

        }

        public void ChangeCell(Cell cell) //"поедание" фигуры
        {
            cell.Color = CurCell.Color;
            CurCell.Color = CellColor.White;
            CurCell = null;
            Board.ClearArea(CurPlayer);       // очищаем пути //ход завершен
            NextPlayer(CurPlayer);      //передаем ход следущему игроку
        }

       

        public Board Board
        {
            get => _board;
            set
            {
                this.RaiseAndSetIfChanged(ref _board, value);
            }
        }


        public void StartGame()
        {
            InitPlayers(SystemSettings.CountPlayers = SystemSettings.BufCountPlayers);
            SetupBoard();
        }


        private void InitPlayers(int CountPlayers)
        {
            if (PlayersList != null) PlayersList.Clear();
            //PlayersList.Add(RedPlayer = new(CellColor.Red, CellColor.DarkRed, true));
            //PlayersList.Add(BluePlayer = new(CellColor.Blue, CellColor.DarkBlue, true));

            //PlayersList.Add(GreenPlayer = new(CellColor.Green, CellColor.DarkGreen, true));
            //PlayersList.Add(YellowPlayer = new(CellColor.Yellow, CellColor.DarkYellow, true));

            RedPlayer = new(CellColor.Red, CellColor.DarkRed, true);
            BluePlayer = new(CellColor.Blue, CellColor.DarkBlue, true);
            GreenPlayer = new(CellColor.Green, CellColor.DarkGreen, true);
            YellowPlayer = new(CellColor.Yellow, CellColor.DarkYellow, true);

            PlayersList.Add(RedPlayer);
            PlayersList.Add(BluePlayer);
            PlayersList.Add(GreenPlayer);
            PlayersList.Add(YellowPlayer);

            PlayersList.IndexOf(GreenPlayer);

            SystemSettings.CountActivePlayers = 4;

            switch (CountPlayers)
            {
                case 2:
                    GreenPlayer.Active = false;
                    SystemSettings.CountActivePlayers -= 1;

                    goto case 3;
                case 3:
                    YellowPlayer.Active = false;
                    SystemSettings.CountActivePlayers -= 1;
                    break;
            }
            CurPlayer = RedPlayer;
            SystemSettings.StepsOnMotion = SystemSettings.BufPlayersSteps;
            CurPlayer.StepsLeft = SystemSettings.StepsOnMotion; //осталось макc кол-во шагов

            GameState = State.Ready;
        }



        private void NextPlayer(Player curPlayer) //смена очередности хода
        {
            if (PlayersList.IndexOf(CurPlayer) == 3) CurPlayer = PlayersList[0];
            else CurPlayer = PlayersList[PlayersList.IndexOf(CurPlayer) + 1];
            
            if (!CurPlayer.Active)
                NextPlayer(CurPlayer);
            else 
                CurPlayer.StepsLeft = SystemSettings.StepsOnMotion; //осталось макc кол-во шагов
        }

        private void SetupBoard()
        {
            IsGameOver = false;
            Board board = new Board();
            board[0, 4] = RedPlayer;
            board[0, 5] = RedPlayer;
            board[0, 6] = RedPlayer;
            board[0, 7] = RedPlayer;
            if (GreenPlayer.Active)
            {
                board[4, 0] = GreenPlayer;
                board[5, 0] = GreenPlayer;
                board[6, 0] = GreenPlayer;
                board[7, 0] = GreenPlayer;
            }
            if (YellowPlayer.Active)
            {
                board[4, 11] = YellowPlayer;
                board[5, 11] = YellowPlayer;
                board[6, 11] = YellowPlayer;
                board[7, 11] = YellowPlayer;
            }
            board[11, 4] = BluePlayer;
            board[11, 5] = BluePlayer;
            board[11, 6] = BluePlayer;
            board[11, 7] = BluePlayer; 
            Board = board;

            GameState = State.Going;
        }
    }
}
