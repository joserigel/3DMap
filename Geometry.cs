
namespace Engine {
    class Geometry {
        public static Geometry CreateSphere(float diameter, uint segments, uint rings) {
            float radius = diameter / 2;

            //Calculate Vertices
            float[] vertices = new float[
                (2 + ((segments + 1) * (rings - 1))) * 9
            ];

            uint idx = 1;
            float uvY = 0f;
            for (float y = -radius; y < radius; y+= diameter/rings) {
                float angle = (MathF.PI * 2) / segments;
                for (uint i=0; i<=segments; i++) {
                    float xTrig = MathF.Cos(i * angle);
                    float zTrig = MathF.Sin(i * angle);

                    //Coordinates
                    vertices[idx]     = xTrig * radius;
                    vertices[idx + 1] = y;
                    vertices[idx + 2] = zTrig * radius;

                    //UV
                    vertices[idx + 3] = i * (1/segments);
                    vertices[idx + 4] = uvY * (1/rings);

                    //Normal
                    vertices[idx + 5] = xTrig;
                    vertices[idx + 6] = 0;
                    vertices[idx + 7] = zTrig;
                }
                idx +=9;
                uvY++;
            }


            uint[] indices = new uint[
                (rings - 2) * (segments) * 6
            ]; 

            idx = 1;
            for (uint i=0; i<rings - 2; i++) {
                for (uint j=0; j<segments; j++) {
                    indices[idx + (j * 6) + 0] = (idx + j * 6);
                    indices[idx + (j * 6) + 1] = (idx + j * 6) + segments + 1;
                    indices[idx + (j * 6) + 2] = (idx + j * 6) + 1;

                    indices[idx + (j * 6) + 3] = (idx + j * 6) + 1;
                    indices[idx + (j * 6) + 4] = (idx + j * 6) + segments + 1;
                    indices[idx + (j * 6) + 5] = (idx + j * 6) + segments + 2;
                }
                idx += (rings * (segments + 1)) * 6;
            }
            return new Geometry(vertices, indices);
        } 

        private float[] vertices;
        private uint[] indices;

        public Geometry(float[] vertices, uint[] indices) {
            this.vertices = vertices;
            this.indices = indices;
        }
    }
}