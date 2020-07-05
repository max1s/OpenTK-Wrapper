using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenSeasProject
{
    public static class Helper
    {
        public static void Add(this List<float> self, Vector3 vector)
        {
            self.Add(vector.X);
            self.Add(vector.Y);
            self.Add(vector.Z);
        }

        public static void Add(this List<float> self, Vector2 vector)
        {
            self.Add(vector.X);
            self.Add(vector.Y);

        }
    }
}
