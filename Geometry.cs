
namespace Engine {
    class Geometry {
        public static Geometry CreateSphere(float diameter, uint segments, uint rings) {
            float radius = diameter / 2;

            //Calculate Vertices
            float[] vertices = new float[
                ((rings) * (segments + 1) * 8)
            ];
            
            //Vertex Calculations
            uint idx = (segments + 1) * 8;
            float yaw = (MathF.PI * 2) / segments;
            float pitchAdd = MathF.PI / ((float)rings - 1);

            float uvXSegment = 1 / (float)segments;
            float uvYSegment = 1 / (float)(rings - 1);

            //Bottom Vertex
            for (uint i=0; i<=segments; i++) {
                //Coordinates
                vertices[(i * 8) + 1] = -radius;
                
                //UV
                vertices[(i * 8) + 3] = uvXSegment * i;
                vertices[(i * 8) + 4] = 0f;

                //Normals
                vertices[(i * 8) + 6] = -1.0f;
            }

            //Middle Vertices
            float pitch = -(MathF.PI/2f) + pitchAdd;
            for (int i=-(int)(rings/2) + 1; i<(rings/2) - 1; i++) {
                float y = MathF.Sin(pitch);
                float uvY = uvYSegment * (i + (rings/2));

                for (int j=0; j<=segments; j++) {
                    //Trigonometry calculations
                    float xTrig = MathF.Cos(yaw * j);
                    float yTrig = MathF.Cos(pitch);
                    float zTrig = MathF.Sin(yaw * j);

                    float x = xTrig * yTrig;
                    float z = zTrig * yTrig;
                    //Coordinates
                    vertices[idx] = x * radius;
                    vertices[idx + 1] = y * radius;
                    vertices[idx + 2] = z * radius;

                    //UV
                    vertices[idx + 3] = uvXSegment * j;
                    vertices[idx + 4] = uvY;

                    //Normals
                    vertices[idx + 5] = x;
                    vertices[idx + 6] = y;
                    vertices[idx + 7] = z;

                    idx+=8;
                }
                pitch += pitchAdd;
            }

            //Top Vertex
            for (int i=0; idx<vertices.Length; idx+=8, i++) {
                //Coordinates
                vertices[idx + 1] = radius;
                
                //UV
                vertices[idx + 3] = uvXSegment * i;
                vertices[idx + 4] = 1f;

                //Normals
                vertices[idx + 6] = 1.0f;
            }

            uint[] indices = new uint[
                (rings) * (segments) * 6
            ]; 
            
            idx = 0; uint offset = 0;
            for (int i=0; i<rings; i++) {
                for (int j=0; j<segments; j++) {
                    
                    indices[idx] = offset;
                    indices[idx + 1] = offset + segments + 1;
                    indices[idx + 2] = offset + 1;

                    indices[idx + 3] = offset + 1;
                    indices[idx + 4] = offset + segments + 1;
                    indices[idx + 5] = offset + segments + 2;
                    idx+=6;
                    offset++;
                }
            }


            //Tracing!!!!!!
            /*
            for (int i=0; i<indices.Length; i+=3) {
                Console.WriteLine(string.Format("{0}: {1}, {2}, {3}",
                    i/3,
                    indices[i],
                    indices[i + 1],
                    indices[i + 2]
                ));
            }*/
            
            for (int i=0; i<vertices.Length; i+=8) {
                Console.WriteLine(string.Format("{0}:\t{1:0.##}, {2:0.##}, {3:0.##}  \t\t\t{4:0.##},{5:0.##}\t\t{6:0.##},{7:0.##},{8:0.##}",
                    i/8,
                    vertices[i],
                    vertices[i + 1],
                    vertices[i + 2],
                    vertices[i + 3],
                    vertices[i + 4],
                    vertices[i + 5],
                    vertices[i + 6],
                    vertices[i + 7]
                ));
            }
            return new Geometry(vertices, indices);
        } 

        private float[] vertices;
        private uint[] indices;
        public float[] Vertices {
            get {
                return vertices;
            }
            private set {
                vertices = value;
            }
        }
        public uint[] Indices {
            get {
                return indices;
            }
            private set {
                indices = value;
            }
        }

        public Geometry(float[] vertices, uint[] indices) {
            this.vertices = vertices;
            this.indices = indices;
        }
    }
}