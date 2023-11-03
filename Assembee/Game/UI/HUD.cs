using Assembee.Game.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game.Entities
{
    static class HUD {
        static Color col = Color.Black;

        private static Root root = new Root();

        private static Frame gameUI = new Frame(root);

        private static Panel buildPanel = new Panel(gameUI, Vector2.Zero, new Vector2(100, 100), Element.Orientation.LowerLeft);
        private static Text buildText = new Text(buildPanel, "Building:", Vector2.Zero, 0.8f, Element.Orientation.Center);

        private static Panel beeLinePanel = new Panel(gameUI, Vector2.Zero, new Vector2(100, 100), Element.Orientation.LowerLeft);
        private static Text beeLineTextA = new Text(beeLinePanel, "Place bee line start", Vector2.Zero, 0.8f, Element.Orientation.Center);
        private static Text beeLineTextB = new Text(beeLinePanel, "Place bee line end", Vector2.Zero, 0.8f, Element.Orientation.Center);

        private static Panel tilePanel = new Panel(gameUI, new Vector2(0, 0), new Vector2(290, 130), Element.Orientation.UpperRight);
        private static Text tileText = new Text(tilePanel, "Tile:", new Vector2(0, 0), 0.8f, Element.Orientation.Center);

        private static Button muteButton = new Button(gameUI, "Mute", 0.8f, new Vector2(0, 0), new Vector2(110, 50), Element.Orientation.LowerRight);

        private static Frame menuUI = new Frame(root);

        private static Panel menuPanel = new Panel(menuUI, new Vector2(0, 0), new Vector2(100, 100), Element.Orientation.Center);
        private static Text menuText = new Text(menuPanel, "Assembee", new Vector2(0, 0), 1f, Element.Orientation.Center, Game1.font36);
        private static Button newButton = new Button(menuPanel, "New Game", 1.0f, new Vector2(0, 0), new Vector2(10, 10), Element.Orientation.Center);
        private static Button loadButton = new Button(menuPanel, "Load Save", 1.0f, new Vector2(0, 0), new Vector2(10, 10), Element.Orientation.Center);

        public static bool OverActiveElement(Point point) {
            return root.IntersectingGlobalPoint(point);
        }

        public static bool OverActiveElement(Vector2 position) {
            return root.IntersectingGlobalPoint(position.ToPoint());
        }

        public static void InitHUD(World world) {
            newButton.FitToChildren(10.0f);
            loadButton.FitToChildren(10.0f);
            menuPanel.PackVert(10.0f);

            newButton.AssignFunction(world.NewGame);
            loadButton.AssignFunction(world.LoadGame);

            muteButton.AssignFunction(Game1.audio.ToggleMute);

            beeLinePanel.FitToChildren(10.0f);
        }

        public static void DrawMenu(SpriteBatch spriteBatch) {
            root.updateSize();
            menuUI.active = true;
            gameUI.active = false;

            root.DrawAll(spriteBatch);
        }

        public static void DrawHud(SpriteBatch spriteBatch, World world, Game1.Building building) {

            root.updateSize();

            menuUI.active = false;
            gameUI.active = true;

            if (Game1.gameState == Game1.GameState.BuildingTiles) {
                string buildingString;
                switch (building) {
                    case Game1.Building.HoneyProducer:
                        buildingString = IBuildable.CreateUIString(world, GameRegistry.FactoryRegistry[GameRegistry.fct.honeyProduction]);
                        break;

                    case Game1.Building.WaxProducer:
                        buildingString = IBuildable.CreateUIString(world, GameRegistry.FactoryRegistry[GameRegistry.fct.waxProduction]);
                        break;

                    case Game1.Building.Apartment:
                        buildingString = IBuildable.CreateUIString(world, "Apartment", Apartment.StaticBuildingRequirements);
                        break;

                    default:
                        buildingString = "Select Building (1 - 3)";
                        break;
                }
                buildText.SetString(buildingString);
                buildPanel.FitToChildren(10.0f);
                buildPanel.active = true;
            } else {
                buildPanel.active = false;
            }

            if (Game1.gameState == Game1.GameState.BuildingLines) {
                if (Game1.beeLineStart is null) {
                    beeLineTextA.active = true;
                    beeLineTextB.active = false;
                } else {
                    beeLineTextA.active = false;
                    beeLineTextB.active = true;
                }

                beeLinePanel.active = true;
            } else {
                beeLinePanel.active = false;
            }

            if (world != null) {
                if (!(world.SelectedTile is null)) {
                    string tileString = world.SelectedTile.GetInfoString();
                    tileText.SetString(tileString);
                    tilePanel.FitToChildren(10.0f);
                    tilePanel.active = true;
                } else {
                    tilePanel.active = false;
                }
            }

            muteButton.SetString(Game1.audio.Muted ? "Unmute" : "Mute");

            root.DrawAll(spriteBatch);
        }
    }
}
