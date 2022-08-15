using System;
using System.Collections.Generic;
using System.Text;

namespace JPEG_Cs
{
    /// <summary>
    /// все возможные типы маркеров jpeg
    /// </summary>
    public enum ТипМаркера
    {
        BaselineDCT_n_dHuffman = 0xFFC0,
        ExtendedSeqDCT_n_dHuffman = 0xFFC1,
        ProgressiveDCT_n_dHuffman = 0xFFC2,
        Lossless_n_dHuffman = 0xFFC3,

        DifseqDCT_dHuffman = 0xFFC5,
        DifprogDCT_dHuffman = 0xFFC6,
        DiflosslessDCT_dHuffman = 0xFFC7,

        ResforJPEGext_n_dArithmetic = 0xFFC8,
        ExtendedSeqDCT_n_dArithmetic = 0xFFC9,
        ProgressiveDCT_n_dArithmetic = 0xFFCA,
        Lossless_n_dArithmetic = 0xFFCB,

        DifseqDCT_dArithmetic = 0xFFCD,
        DifprogDCT_dArithmetic = 0xFFCE,
        DiflosslessDCT_dArithmetic = 0xFFCF,

        DefineHuffmantebles = 0xFFC4,

        DefineArithcodingcond = 0xFFCC,

        Restartwithmodulo8countm0 = 0xFFD0,
        Restartwithmodulo8countm1 = 0xFFD1,
        Restartwithmodulo8countm2 = 0xFFD2,
        Restartwithmodulo8countm3 = 0xFFD3,
        Restartwithmodulo8countm4 = 0xFFD4,
        Restartwithmodulo8countm5 = 0xFFD5,
        Restartwithmodulo8countm6 = 0xFFD6,
        Restartwithmodulo8countm7 = 0xFFD7,

        StartofImage = 0xFFD8,
        EndofImage = 0xFFD9,
        StartofSkan = 0xFFDA,
        DefquatTab = 0xFFDB,
        Defnumoflines = 0xFFDC,
        Defresint = 0xFFDD,
        Defhieprog = 0xFFDE,
        Exprefcomp = 0xFFDF,
        ReservedForAppSegment = 0xFFE0,
        ResforJPEGext = 0xFFE0,
        Comment = 0XFFFE,
        FortempprivuseianArithcoding = 0xFF01,
        Reserved = 0xFF02,
    }
}
