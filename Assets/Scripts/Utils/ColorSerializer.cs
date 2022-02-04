using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ColorSerializer
{
    public static byte[] Serialize(object targetObject)
    {
        Color color = (Color)targetObject;

        Quaternion colorToQuaternion = new Quaternion(color.r, color.g, color.b, color.a);
        byte[] bytes = Protocol.Serialize(colorToQuaternion);

        return bytes;
    }

    public static object Deserialize(byte[] bytes)
    {
        Quaternion quaternion = (Quaternion)Protocol.Deserialize(bytes);
        Color color = new Color(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        return color;
    }
}

