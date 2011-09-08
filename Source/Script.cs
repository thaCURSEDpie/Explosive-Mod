

namespace Bombit.Source
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using GTA;

    public class BombScript : Script
    {
        private List<CBomb> bombs;
        private int bombCounter;

        private EBombType currentBombType = 0;

        private EExplosionType currentExplosionType = 0;

        private EDetonationMethod detMethod = EDetonationMethod.Parallel;

        private CRocket rocket;

        private bool rocketActive = false;

        private GTA.Object testObj;

        public BombScript()
        {
            GVars.PlayerPed = Player.Character;
            this.bombs = new List<CBomb>();
            this.bombCounter = 0;

            this.KeyDown += new GTA.KeyEventHandler(this.BombScript_KeyDown);
            this.Tick += new EventHandler(this.BombScript_Tick);

            this.Interval = 500;
            testObj = World.CreateObject("EC_BOMB_NE", Player.Character.GetOffsetPosition(new Vector3(15f, 0f, 0f)));
        }

        private void BombScript_Tick(object sender, EventArgs e)
        {
            if (testObj != null && testObj.Exists())
            {
                testObj.Collision = false;
                testObj.Position = Player.Character.GetOffsetPosition(new Vector3(15f, 0f, 0f));                
            }
            GVars.AllPeds = World.GetAllPeds();
            //GVars.AllObjs = World.GetAllObjects();
            //GVars.AllVehs = World.GetVehicles(GVars.PlayerPed.Position, GParams.VehSearchRange);

            lock (this.bombs)
            {
                for (int i = 0; i < this.bombs.Count; i++)
                {
                    if (this.bombs[i] != null)
                    {
                        if (this.bombs[i].State == EBombState.Exploded)
                        {
                            this.bombs[i].Delete();
                            Wait(1);
                            this.bombs.RemoveAt(i);
                        }
                    }
                }
            }

            if (this.rocket != null)
            {
                if (this.rocket.State == ERocketState.Done || this.rocket.Bomb.State == EBombState.Exploded)
                {
                    this.rocket.Delete();
                    Wait(1);
                    this.rocket = null;
                    rocketActive = false;
                }
            }
        }

        private void BombScript_KeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (e.Key == GParams.RocketStartKey)
            {
                if (!rocketActive && Game.isGameKeyPressed(GameKey.Aim) && !Player.Character.isInVehicle())
                {
                    this.rocket = new CRocket(Game.CurrentCamera.Position + Game.CurrentCamera.Direction * 3.0f, Game.CurrentCamera.Direction, this.currentBombType, this.currentExplosionType, 5, 0);
                    rocketActive = true;
                }
            }
            else if (e.Key == GParams.BombPlaceKey)
            {
                Vector3 pos = Player.Character.Position;
                
                lock (this.bombs)
                {
                    // Too many bombs: start over
                    if (this.bombCounter > GParams.MaxBombs)
                    {
                        this.bombCounter = 0;

                        if (this.bombs[0] != null)
                        {
                            this.bombs[0].Delete();
                            this.bombs.RemoveAt(0);
                        }
                    }

                    this.bombs.Add(new CBomb(Player.Character.GetOffsetPosition(new Vector3(0f, 1f, 0f)).ToGround(),
                                                             "EC_BOMB_NE",
                                                             this.currentBombType,
                                                             this.currentExplosionType,
                                                             5));
                    this.bombCounter++;
                }
            }
            else if (e.Key == GParams.BombDetonateKey)
            {
                if (this.detMethod == EDetonationMethod.Parallel)
                {
                    GVars.DetonateTime = System.DateTime.Now.Ticks;
                }
                else if (this.detMethod == EDetonationMethod.Serial)
                {
                    for (int i = 0; i < this.bombs.Count; i++)
                    {
                        if (this.bombs[i].Type == EBombType.manual)
                        {
                            this.bombs[i].Explode();
                            Wait(20);
                            this.bombs[i].Delete();
                            Wait(10);
                        }                        
                    }
                }
            }
            else if (e.Key == GParams.BombChangeTypeKey)
            {
                if ((int)this.currentBombType + 1 < GParams.NumBombTypes)
                {
                    this.currentBombType++;
                }
                else
                {
                    this.currentBombType = 0;
                }

                Game.DisplayText("Bomb type: " + this.currentBombType.ToString(), 1800);
            }
            else if (e.Key == GParams.DetTypeChangeKey)
            {
                if ((int)this.detMethod + 1 < GParams.NumDetTypes)
                {
                    this.detMethod++;
                }
                else
                {
                    this.detMethod = 0;
                }

                Game.DisplayText("Det method: " + this.detMethod.ToString(), 1800);
            }
            else if (e.Key == GParams.ExTypeChangeKey)
            {
                if ((int)this.currentExplosionType + 1 < GParams.NumExTypes)
                {
                    this.currentExplosionType++;
                }
                else
                {
                    this.currentExplosionType = 0;
                }

                Game.DisplayText("Explosion type: " + this.currentExplosionType.ToString(), 1800);
            }
        }
    }
}
