﻿#region license
// Copyright (C) 2020 ClassicUO Development Community on Github
// 
// This project is an alternative client for the game Ultima Online.
// The goal of this is to develop a lightweight client considering
// new technologies.
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.Threading.Tasks;

using ClassicUO.Game;
using ClassicUO.Renderer;
using ClassicUO.Utility;

namespace ClassicUO.IO.Resources
{
    internal class LightsLoader : UOFileLoader<UOTexture16>
    {
        private UOFileMul _file;

        protected LightsLoader(int count) : base(count)
        {

        }

        public static LightsLoader _instance;
        public static LightsLoader Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LightsLoader(Constants.MAX_LIGHTS_DATA_INDEX_COUNT);
                }

                return _instance;
            }
        }

        public override Task Load()
        {
            return Task.Run(() => {
                string path = UOFileManager.GetUOFilePath("light.mul");
                string pathidx = UOFileManager.GetUOFilePath("lightidx.mul");

                FileSystemHelper.EnsureFileExists(path);
                FileSystemHelper.EnsureFileExists(pathidx);

                _file = new UOFileMul(path, pathidx, Constants.MAX_LIGHTS_DATA_INDEX_COUNT);
                _file.FillEntries(ref Entries);

            });
        }


        public override void CleanResources()
        {
            base.CleanResources();
            
            _file?.Dispose();
            _file = null;
            _instance = null;
        }

        public override UOTexture16 GetTexture(uint id, bool keepData = false)
        {
            if (id >= Resources.Length)
                return null;

            ref var texture = ref Resources[id];

            if (texture == null || texture.IsDisposed)
            {
                ushort[] pixels = GetLight(id, out int w, out int h);

                texture = new UOTexture16(w, h);
                texture.PushData(pixels);

                SaveID(id);
            }

            return texture;
        }


        private ushort[] GetLight(uint idx, out int width, out int height)
        {
            ref readonly var entry = ref GetValidRefEntry((int) idx);

            width = entry.Width;
            height = entry.Height;

            ushort[] pixels = new ushort[width * height];

            _file.Seek(entry.Offset);

            for (int i = 0; i < height; i++)
            {
                int pos = i * width;

                for (int j = 0; j < width; j++)
                {
                    ushort val = _file.ReadByte();
                    val = (ushort) ((val << 10) | (val << 5) | val);
                    pixels[pos + j] = (ushort) ((val != 0 ? 0x8000 : 0) | val);
                }
            }

            return pixels;
        }
    }
}