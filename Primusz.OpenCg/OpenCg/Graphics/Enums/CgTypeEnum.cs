﻿using System;

namespace OpenCg.Graphics
{
    [Serializable]
    public enum CgType
    {
        UnknownType = 0,
        Array = 2,
        String = 1135,
        Struct = 1,
        TypelessStruct = 3,
        Texture = 1137,
        Buffer = 1319,
        UniformBuffer = 1320,
        Address = 1321,
        PixelshaderType = 1142,
        ProgramType = 1136,
        VertexshaderType = 1141,
        Sampler = 1143,
        Sampler1d = 1065,
        Sampler1dArray = 1138,
        Sampler1dShadow = 1313,
        Sampler2d = 1066,
        Sampler2dArray = 1139,
        Sampler2dms = 1317,      /* ArbTextureMultisample */
        Sampler2dmsArray = 1318, /* ArbTextureMultisample */
        Sampler2dShadow = 1314,
        Sampler3d = 1067,
        Samplerbuf = 1144,
        Samplercube = 1069,
        SamplercubeArray = 1140,
        Samplerrbuf = 1316,      /* NvExplicitMultisample */
        Samplerrect = 1068,
        SamplerrectShadow = 1315,
        TypeStartEnum = 1024,
        Bool = 1114,
        Bool1 = 1115,
        Bool2 = 1116,
        Bool3 = 1117,
        Bool4 = 1118,
        Bool1x1 = 1119,
        Bool1x2 = 1120,
        Bool1x3 = 1121,
        Bool1x4 = 1122,
        Bool2x1 = 1123,
        Bool2x2 = 1124,
        Bool2x3 = 1125,
        Bool2x4 = 1126,
        Bool3x1 = 1127,
        Bool3x2 = 1128,
        Bool3x3 = 1129,
        Bool3x4 = 1130,
        Bool4x1 = 1131,
        Bool4x2 = 1132,
        Bool4x3 = 1133,
        Bool4x4 = 1134,
        Char = 1166,
        Char1 = 1167,
        Char2 = 1168,
        Char3 = 1169,
        Char4 = 1170,
        Char1x1 = 1171,
        Char1x2 = 1172,
        Char1x3 = 1173,
        Char1x4 = 1174,
        Char2x1 = 1175,
        Char2x2 = 1176,
        Char2x3 = 1177,
        Char2x4 = 1178,
        Char3x1 = 1179,
        Char3x2 = 1180,
        Char3x3 = 1181,
        Char3x4 = 1182,
        Char4x1 = 1183,
        Char4x2 = 1184,
        Char4x3 = 1185,
        Char4x4 = 1186,
        Double = 1145,
        Double1 = 1146,
        Double2 = 1147,
        Double3 = 1148,
        Double4 = 1149,
        Double1x1 = 1150,
        Double1x2 = 1151,
        Double1x3 = 1152,
        Double1x4 = 1153,
        Double2x1 = 1154,
        Double2x2 = 1155,
        Double2x3 = 1156,
        Double2x4 = 1157,
        Double3x1 = 1158,
        Double3x2 = 1159,
        Double3x3 = 1160,
        Double3x4 = 1161,
        Double4x1 = 1162,
        Double4x2 = 1163,
        Double4x3 = 1164,
        Double4x4 = 1165,
        Fixed = 1070,
        Fixed1 = 1092,
        Fixed2 = 1071,
        Fixed3 = 1072,
        Fixed4 = 1073,
        Fixed1x1 = 1074,
        Fixed1x2 = 1075,
        Fixed1x3 = 1076,
        Fixed1x4 = 1077,
        Fixed2x1 = 1078,
        Fixed2x2 = 1079,
        Fixed2x3 = 1080,
        Fixed2x4 = 1081,
        Fixed3x1 = 1082,
        Fixed3x2 = 1083,
        Fixed3x3 = 1084,
        Fixed3x4 = 1085,
        Fixed4x1 = 1086,
        Fixed4x2 = 1087,
        Fixed4x3 = 1088,
        Fixed4x4 = 1089,
        Float = 1045,
        Float1 = 1091,
        Float2 = 1046,
        Float3 = 1047,
        Float4 = 1048,
        Float1x1 = 1049,
        Float1x2 = 1050,
        Float1x3 = 1051,
        Float1x4 = 1052,
        Float2x1 = 1053,
        Float2x2 = 1054,
        Float2x3 = 1055,
        Float2x4 = 1056,
        Float3x1 = 1057,
        Float3x2 = 1058,
        Float3x3 = 1059,
        Float3x4 = 1060,
        Float4x1 = 1061,
        Float4x2 = 1062,
        Float4x3 = 1063,
        Float4x4 = 1064,
        Half = 1025,
        Half1 = 1090,
        Half2 = 1026,
        Half3 = 1027,
        Half4 = 1028,
        Half1x1 = 1029,
        Half1x2 = 1030,
        Half1x3 = 1031,
        Half1x4 = 1032,
        Half2x1 = 1033,
        Half2x2 = 1034,
        Half2x3 = 1035,
        Half2x4 = 1036,
        Half3x1 = 1037,
        Half3x2 = 1038,
        Half3x3 = 1039,
        Half3x4 = 1040,
        Half4x1 = 1041,
        Half4x2 = 1042,
        Half4x3 = 1043,
        Half4x4 = 1044,
        Int = 1093,
        Int1 = 1094,
        Int2 = 1095,
        Int3 = 1096,
        Int4 = 1097,
        Int1x1 = 1098,
        Int1x2 = 1099,
        Int1x3 = 1100,
        Int1x4 = 1101,
        Int2x1 = 1102,
        Int2x2 = 1103,
        Int2x3 = 1104,
        Int2x4 = 1105,
        Int3x1 = 1106,
        Int3x2 = 1107,
        Int3x3 = 1108,
        Int3x4 = 1109,
        Int4x1 = 1110,
        Int4x2 = 1111,
        Int4x3 = 1112,
        Int4x4 = 1113,
        Long = 1271,
        Long1 = 1272,
        Long2 = 1273,
        Long3 = 1274,
        Long4 = 1275,
        Long1x1 = 1276,
        Long1x2 = 1277,
        Long1x3 = 1278,
        Long1x4 = 1279,
        Long2x1 = 1280,
        Long2x2 = 1281,
        Long2x3 = 1282,
        Long2x4 = 1283,
        Long3x1 = 1284,
        Long3x2 = 1285,
        Long3x3 = 1286,
        Long3x4 = 1287,
        Long4x1 = 1288,
        Long4x2 = 1289,
        Long4x3 = 1290,
        Long4x4 = 1291,
        Short = 1208,
        Short1 = 1209,
        Short2 = 1210,
        Short3 = 1211,
        Short4 = 1212,
        Short1x1 = 1213,
        Short1x2 = 1214,
        Short1x3 = 1215,
        Short1x4 = 1216,
        Short2x1 = 1217,
        Short2x2 = 1218,
        Short2x3 = 1219,
        Short2x4 = 1220,
        Short3x1 = 1221,
        Short3x2 = 1222,
        Short3x3 = 1223,
        Short3x4 = 1224,
        Short4x1 = 1225,
        Short4x2 = 1226,
        Short4x3 = 1227,
        Short4x4 = 1228,
        Uchar = 1187,
        Uchar1 = 1188,
        Uchar2 = 1189,
        Uchar3 = 1190,
        Uchar4 = 1191,
        Uchar1x1 = 1192,
        Uchar1x2 = 1193,
        Uchar1x3 = 1194,
        Uchar1x4 = 1195,
        Uchar2x1 = 1196,
        Uchar2x2 = 1197,
        Uchar2x3 = 1198,
        Uchar2x4 = 1199,
        Uchar3x1 = 1200,
        Uchar3x2 = 1201,
        Uchar3x3 = 1202,
        Uchar3x4 = 1203,
        Uchar4x1 = 1204,
        Uchar4x2 = 1205,
        Uchar4x3 = 1206,
        Uchar4x4 = 1207,
        Uint = 1250,
        Uint1 = 1251,
        Uint2 = 1252,
        Uint3 = 1253,
        Uint4 = 1254,
        Uint1x1 = 1255,
        Uint1x2 = 1256,
        Uint1x3 = 1257,
        Uint1x4 = 1258,
        Uint2x1 = 1259,
        Uint2x2 = 1260,
        Uint2x3 = 1261,
        Uint2x4 = 1262,
        Uint3x1 = 1263,
        Uint3x2 = 1264,
        Uint3x3 = 1265,
        Uint3x4 = 1266,
        Uint4x1 = 1267,
        Uint4x2 = 1268,
        Uint4x3 = 1269,
        Uint4x4 = 1270,
        Ulong = 1292,
        Ulong1 = 1293,
        Ulong2 = 1294,
        Ulong3 = 1295,
        Ulong4 = 1296,
        Ulong1x1 = 1297,
        Ulong1x2 = 1298,
        Ulong1x3 = 1299,
        Ulong1x4 = 1300,
        Ulong2x1 = 1301,
        Ulong2x2 = 1302,
        Ulong2x3 = 1303,
        Ulong2x4 = 1304,
        Ulong3x1 = 1305,
        Ulong3x2 = 1306,
        Ulong3x3 = 1307,
        Ulong3x4 = 1308,
        Ulong4x1 = 1309,
        Ulong4x2 = 1310,
        Ulong4x3 = 1311,
        Ulong4x4 = 1312,
        Ushort = 1229,
        Ushort1 = 1230,
        Ushort2 = 1231,
        Ushort3 = 1232,
        Ushort4 = 1233,
        Ushort1x1 = 1234,
        Ushort1x2 = 1235,
        Ushort1x3 = 1236,
        Ushort1x4 = 1237,
        Ushort2x1 = 1238,
        Ushort2x2 = 1239,
        Ushort2x3 = 1240,
        Ushort2x4 = 1241,
        Ushort3x1 = 1242,
        Ushort3x2 = 1243,
        Ushort3x3 = 1244,
        Ushort3x4 = 1245,
        Ushort4x1 = 1246,
        Ushort4x2 = 1247,
        Ushort4x3 = 1248,
        Ushort4x4 = 1249
    }
}