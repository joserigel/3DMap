
namespace Engine {
    class Geometry {
        public static Geometry CreateSphere(float diameter, uint segments, uint rings) {
            float radius = diameter / 2;

            float[] vertices = new float[
                (((rings - 2) * (segments + 1)) + (2 * segments))  * 8
            ];
            
            float uvXSegment = 1 / (float)segments;
            float uvXFan = 1 / ((float)segments - 1); 
            float uvYSegment = 1 / ((float)rings - 2);

            float yaw = (MathF.PI * 2) / segments;
            float pitchAdd = MathF.PI / ((float)rings - 1);
            float pitch = -(MathF.PI/2f) + pitchAdd;

            uint idx = 0;

            //Bottom Vertices
            for (int i=0; i<segments; i++)  {

                //Coordinates
                vertices[idx] = 0f;
                vertices[idx + 1] = -radius;
                vertices[idx + 2] = 0f;

                //UV
                vertices[idx + 3] = 1 - (i * uvXFan);
                vertices[idx + 4] = 0f;

                //Normals
                vertices[idx + 5] = 0f;
                vertices[idx + 6] = 0f;
                vertices[idx + 7] = 0f;

                idx += 8;
            }

            //Middle Vertices
            for (uint i=0; i<rings-2; i++) {
                float y = MathF.Sin(pitch);
                float uvY = uvYSegment * (i + 1);
                Console.WriteLine(uvY);
                for (uint j=0; j<=segments; j++) {
                    //Trignometry Calculations
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
                    vertices[idx + 3] = 1 - (j * uvXSegment);
                    vertices[idx + 4] = uvY;
                    
                    //Normals
                    vertices[idx + 5] = x;
                    vertices[idx + 6] = y;
                    vertices[idx + 7] = z;

                    idx += 8;
                }
                pitch += pitchAdd;
            }

            //Top Vertices
            for (int i=0; i<segments; i++) {
                //Coordinates
                vertices[idx] = 0f;
                vertices[idx + 1] = radius;
                vertices[idx + 2] = 0f;

                //UV
                vertices[idx + 3] = 1 - (i * uvXFan);
                vertices[idx + 4] = 1f;

                //Normals
                vertices[idx + 5] = 0f;
                vertices[idx + 6] = 1f;
                vertices[idx + 7] = 0f;

                idx += 8;
            }

            uint[] indices = new uint[
                ((rings - 3) * segments * 6) + (2 * segments * 3)
            ];
            
            idx = 0; uint offset = 0;

            //Bottom Triangles
            for (int i=0; i<segments; i++) {
                indices[idx] = offset;
                indices[idx + 1] = offset + segments;
                indices[idx + 2] = offset + segments + 1;

                idx+=3;
                offset++;
            }
            
            //Middle Triangles
            for (int i=0; i<rings-3; i++) {
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
                offset++;
            }
            
            

            //Top Triangles
            idx = (uint)indices.Length - (segments * 3);
            offset = (uint)(vertices.Length/8) - segments - segments - 1;
            for (int i=0; i<segments; i++) {
                indices[idx] = offset;
                indices[idx + 1] = offset + segments + 1;
                indices[idx + 2] = offset + 1;
                
                idx +=3;
                offset++;
            }
            
            
            /*
            for (int i=0; i<vertices.Length; i+=8) {
                Console.WriteLine(i+": "+vertices[i]+", "+vertices[i + 1]+", "+vertices[i + 2]);
            }
            Console.WriteLine("====================================");
            for (int i=0; i<indices.Length; i+=3) {
                Console.WriteLine(i+": "+indices[i]+", "+indices[i + 1]+", "+indices[i + 2]);
            }
            
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
                vertices[i * 8] = i * radius;
                vertices[(i * 8) + 1] = - 16f;
                vertices[(i * 8) + 2] = 10f;
                
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
                    vertices[idx] = j * radius;
                    vertices[idx + 1] = i * radius;
                    vertices[idx + 2] = 0 * radius;

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
                vertices[idx] = i * radius;
                vertices[idx + 1] = 30f;
                vertices[idx + 2] = 10f;
                
                
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
            for (int i=0; i<indices.Length; i+=3) {
                Console.WriteLine(string.Format("{0}: {1}, {2}, {3}",
                    i/3,
                    indices[i],
                    indices[i + 1],
                    indices[i + 2]
                ));
            }
            Console.WriteLine("==================================");
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
            }*/
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