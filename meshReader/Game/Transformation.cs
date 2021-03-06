﻿using meshReader.Game.ADT;
using meshReader.Game.WMO;
using Microsoft.Xna.Framework;

namespace meshReader.Game
{
    
    public static class Transformation
    {
        
        public interface IDefinition
        {
            Vector3 Position { get; }
            float Scale { get; }
            Vector3 Rotation { get; }
        }

        public static Matrix GetTransformation(IDefinition def)
        {
            Matrix translation;
            if (def.Position.X == 0.0f && def.Position.Y == 0.0f && def.Position.Z == 0.0f)
                translation = Matrix.Identity;
            else
                translation = Matrix.CreateTranslation(-(def.Position.Z - Constant.MaxXY),
                                                       -(def.Position.X - Constant.MaxXY), def.Position.Y);

            var rotation = Matrix.CreateRotationX(MathHelper.ToRadians(def.Rotation.Z))*
                           Matrix.CreateRotationY(MathHelper.ToRadians(def.Rotation.X))*Matrix.CreateRotationZ(MathHelper.ToRadians(def.Rotation.Y + 180));

            if (def.Scale < 1.0f || def.Scale > 1.0f)
                return (Matrix.CreateScale(def.Scale)*rotation)*translation;
            return rotation * translation;
        }

        public static Matrix GetWmoDoodadTransformation(DoodadInstance inst, WorldModelHandler.WorldModelDefinition root)
        {
            var rootTransformation = GetTransformation(root);

            var translation = Matrix.CreateTranslation(inst.Position);
            var scale = Matrix.CreateScale(inst.Scale);
            var rotation = Matrix.CreateRotationY(MathHelper.Pi);
            var quatRotation =
                Matrix.CreateFromQuaternion(new Quaternion(-inst.QuatY, inst.QuatZ, -inst.QuatX, inst.QuatW));

            return scale*rotation*quatRotation*translation*rootTransformation;
        }
    }

}