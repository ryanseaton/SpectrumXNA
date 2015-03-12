using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SpectrumXNA
{
    public class Camera
    {
        private Vector3 _position;
        private Vector3 _lookAt;
        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;
        private float _aspectRatio;

        public Camera(Viewport viewport)
        {
            this._aspectRatio = viewport.AspectRatio;
            this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                                        MathHelper.ToRadians(40.0f),
                                        this._aspectRatio,
                                        1.0f,
                                        10000.0f);
        }

        public Vector3 Position
        {
            get { return this._position; }
            set { this._position = value; }
        }
        public Vector3 LookAt
        {
            get { return this._lookAt; }
            set { this._lookAt = value; }
        }
        public Matrix ViewMatrix
        {
            get { return this._viewMatrix; }
        }
        public Matrix ProjectionMatrix
        {
            get { return this._projectionMatrix; }
        }
        public void Update()
        {
            this._viewMatrix =
                Matrix.CreateLookAt(this._position, this._lookAt, Vector3.Up);
        }
    }
}
