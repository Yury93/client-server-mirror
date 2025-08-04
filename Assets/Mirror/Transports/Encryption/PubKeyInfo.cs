using Mirror.BouncyCastle.Crypto;
using System;

namespace Mirror.Transports.Encryption
{
    public struct PubKeyInfo
    {
        public string Fingerprint;
        public ArraySegment<byte> Serialized;
        public AsymmetricKeyParameter Key;
    }
}
