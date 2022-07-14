using K4os.Compression.LZ4;
using System;
using System.IO;

namespace NMSCoordinates
{
    class Class3
    {
        const uint MAGIC_COMPRESSED = 0xFEEDA1E5;

        public static void DecompressSave(string hgFilePath)
        {
            if (hgFilePath.Length == 0)
            {
                Console.WriteLine("ERROR: hgFilePath.Length == 0.");
                return;
            }

            try
            {
                FileStream fs = File.Open(hgFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader br = new BinaryReader(fs);

                uint magicmagic = br.ReadUInt32();
                br.BaseStream.Position = 0;

                if (magicmagic == MAGIC_COMPRESSED)
                {
                    // decompressing
                    MemoryStream mem = new MemoryStream();

                    while (true)
                    {
                        if (br.BaseStream.Position >= br.BaseStream.Length) break;

                        uint magic = br.ReadUInt32();
                        int compressed_size = br.ReadInt32();
                        ulong decompressed_size = br.ReadUInt64();

                        if (magic != 0xFEEDA1E5)
                        {
                            PrintInvalidCompressedBlockError(magic, ref fs);
                            br.Close();
                            fs.Close();
                            return;
                        }

                        byte[] compressed_block = br.ReadBytes(compressed_size);
                        //File.WriteAllBytes("debug_block.bin", compressed_block);

                        byte[] buf = new byte[decompressed_size];
                        int len_decompressed = LZ4Codec.Decode(compressed_block, 0, (int)compressed_block.Length, buf, 0, (int)decompressed_size);
                        mem.Write(buf, 0, buf.Length);

                        //File.WriteAllBytes("debug_block_dec.bin", buf);
                    }
                    br.Close();
                    fs.Close();

                    FileStream file_out = File.Create(hgFilePath);
                    mem.WriteTo(file_out);
                    file_out.Flush();
                    file_out.Close();
                }
                else if (magicmagic == 0x3246227B)
                {
                    Console.WriteLine("magicmagic == 0x3246227B");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine();
            }
        }

        public static void CompressSave(string hgFilePath)
        {
            if (hgFilePath.Length == 0)
            {
                Console.WriteLine("ERROR: hgFilePath.Length == 0.");
                return;
            }

            try
            {
                FileStream fs = File.Open(hgFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryReader br = new BinaryReader(fs);

                uint magicmagic = br.ReadUInt32();
                br.BaseStream.Position = 0;

                if (magicmagic == 0x3246227B)
                {
                    // compressing
                    FileStream file_out = File.Create("_unhg.temp.bin");
                    BinaryWriter writer = new BinaryWriter(file_out);

                    while (true)
                    {
                        // split 0x80000 bytes
                        int len = 0x80000;
                        if (len > (fs.Length - fs.Position))
                        {
                            len = (int)(fs.Length - fs.Position);
                        }

                        Console.WriteLine("Reading {0} bytes at {1}", len, fs.Position);
                        byte[] buf = br.ReadBytes(len);

                        byte[] buf_compressed = new byte[LZ4Codec.MaximumOutputSize(len)];
                        int len_compressed = LZ4Codec.Encode(buf, 0, buf.Length, buf_compressed, 0, buf_compressed.Length);

                        Console.WriteLine("Compressed block of {0} to {1} bytes", len, len_compressed);
                        writer.Write(MAGIC_COMPRESSED);
                        writer.Write(len_compressed);
                        writer.Write((ulong)len);
                        file_out.Write(buf_compressed, 0, len_compressed);
                        file_out.Flush();

                        if (len != 0x80000) break;
                    }

                    file_out.Close();
                    br.Close();
                    fs.Close();

                    File.Copy("_unhg.temp.bin", hgFilePath, true);
                }
                else
                {
                    PrintInvalidFileError();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine();
            }
        }

        private static void PrintInvalidFileError()
        {
            Console.WriteLine("[ERROR] Input is not a No Man's Sky savegame file!");
        }

        private static void PrintInvalidCompressedBlockError(uint totallyWrong, ref FileStream fs)
        {
            Console.WriteLine("[ERROR] {1}: Expected magic 0xFEEDA1E5, got 0x{0:X8} instead.", totallyWrong, fs.Position);
        }
    }
}
