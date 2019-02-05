using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRuettae.Core.Models;

namespace IRuettae.Tuning.LocalSolverGridSearch.DataSets
{
    internal partial class OptimizationDataSets
    {
        private const int Hour = 60 * 60;

        public OptimizationInput[] TwentyDataSets { get; } = new OptimizationInput[20];
        public OptimizationInput[] DataSetsFifteen { get; } = new OptimizationInput[20];

        public OptimizationDataSets()
        {
            TwentyDataSets[0] = DataSet0().input;
            TwentyDataSets[1] = DataSet1().input;
            TwentyDataSets[2] = DataSet2().input;
            TwentyDataSets[3] = DataSet3().input;
            TwentyDataSets[4] = DataSet4().input;
            TwentyDataSets[5] = DataSet5().input;
            TwentyDataSets[6] = DataSet6().input;
            TwentyDataSets[7] = DataSet7().input;
            TwentyDataSets[8] = DataSet8().input;
            TwentyDataSets[9] = DataSet9().input;
            TwentyDataSets[10] = DataSet10().input;
            TwentyDataSets[11] = DataSet11().input;
            TwentyDataSets[12] = DataSet12().input;
            TwentyDataSets[13] = DataSet13().input;
            TwentyDataSets[14] = DataSet14().input;
            TwentyDataSets[15] = DataSet15().input;
            TwentyDataSets[16] = DataSet16().input;
            TwentyDataSets[17] = DataSet17().input;
            TwentyDataSets[18] = DataSet18().input;
            TwentyDataSets[19] = DataSet19().input;
            //TwentyDataSets[20] = DataSet20().input;
            //TwentyDataSets[21] = DataSet21().input;
            //TwentyDataSets[22] = DataSet22().input;
            //TwentyDataSets[23] = DataSet23().input;
            //TwentyDataSets[24] = DataSet24().input;
            //TwentyDataSets[25] = DataSet25().input;
            //TwentyDataSets[26] = DataSet26().input;
            //TwentyDataSets[27] = DataSet27().input;
            //TwentyDataSets[28] = DataSet28().input;
            //TwentyDataSets[29] = DataSet29().input;
            //TwentyDataSets[30] = DataSet30().input;
            //TwentyDataSets[31] = DataSet31().input;
            //TwentyDataSets[32] = DataSet32().input;
            //TwentyDataSets[33] = DataSet33().input;
            //TwentyDataSets[34] = DataSet34().input;
            //TwentyDataSets[35] = DataSet35().input;
            //TwentyDataSets[36] = DataSet36().input;
            //TwentyDataSets[37] = DataSet37().input;
            //TwentyDataSets[38] = DataSet38().input;
            //TwentyDataSets[39] = DataSet39().input;
            //TwentyDataSets[40] = DataSet40().input;
            //TwentyDataSets[41] = DataSet41().input;
            //TwentyDataSets[42] = DataSet42().input;
            //TwentyDataSets[43] = DataSet43().input;
            //TwentyDataSets[44] = DataSet44().input;
            //TwentyDataSets[45] = DataSet45().input;
            //TwentyDataSets[46] = DataSet46().input;
            //TwentyDataSets[47] = DataSet47().input;
            //TwentyDataSets[48] = DataSet48().input;
            //TwentyDataSets[49] = DataSet49().input;
            //TwentyDataSets[50] = DataSet50().input;
            //TwentyDataSets[51] = DataSet51().input;
            //TwentyDataSets[52] = DataSet52().input;
            //TwentyDataSets[53] = DataSet53().input;
            //TwentyDataSets[54] = DataSet54().input;
            //TwentyDataSets[55] = DataSet55().input;
            //TwentyDataSets[56] = DataSet56().input;
            //TwentyDataSets[57] = DataSet57().input;
            //TwentyDataSets[58] = DataSet58().input;
            //TwentyDataSets[59] = DataSet59().input;
            //TwentyDataSets[60] = DataSet60().input;
            //TwentyDataSets[61] = DataSet61().input;
            //TwentyDataSets[62] = DataSet62().input;
            //TwentyDataSets[63] = DataSet63().input;
            //TwentyDataSets[64] = DataSet64().input;
            //TwentyDataSets[65] = DataSet65().input;
            //TwentyDataSets[66] = DataSet66().input;
            //TwentyDataSets[67] = DataSet67().input;
            //TwentyDataSets[68] = DataSet68().input;
            //TwentyDataSets[69] = DataSet69().input;
            //TwentyDataSets[70] = DataSet70().input;
            //TwentyDataSets[71] = DataSet71().input;
            //TwentyDataSets[72] = DataSet72().input;
            //TwentyDataSets[73] = DataSet73().input;
            //TwentyDataSets[74] = DataSet74().input;
            //TwentyDataSets[75] = DataSet75().input;
            //TwentyDataSets[76] = DataSet76().input;
            //TwentyDataSets[77] = DataSet77().input;
            //TwentyDataSets[78] = DataSet78().input;
            //TwentyDataSets[79] = DataSet79().input;
            //TwentyDataSets[80] = DataSet80().input;
            //TwentyDataSets[81] = DataSet81().input;
            //TwentyDataSets[82] = DataSet82().input;
            //TwentyDataSets[83] = DataSet83().input;
            //TwentyDataSets[84] = DataSet84().input;
            //TwentyDataSets[85] = DataSet85().input;
            //TwentyDataSets[86] = DataSet86().input;
            //TwentyDataSets[87] = DataSet87().input;
            //TwentyDataSets[88] = DataSet88().input;
            //TwentyDataSets[89] = DataSet89().input;
            //TwentyDataSets[90] = DataSet90().input;
            //TwentyDataSets[91] = DataSet91().input;
            //TwentyDataSets[92] = DataSet92().input;
            //TwentyDataSets[93] = DataSet93().input;
            //TwentyDataSets[94] = DataSet94().input;
            //TwentyDataSets[95] = DataSet95().input;
            //TwentyDataSets[96] = DataSet96().input;
            //TwentyDataSets[97] = DataSet97().input;
            //TwentyDataSets[98] = DataSet98().input;
            //TwentyDataSets[99] = DataSet99().input;


            DataSetsFifteen[0] = DataSetFifteen0().input;
            DataSetsFifteen[1] = DataSetFifteen1().input;
            DataSetsFifteen[2] = DataSetFifteen2().input;
            DataSetsFifteen[3] = DataSetFifteen3().input;
            DataSetsFifteen[4] = DataSetFifteen4().input;
            DataSetsFifteen[5] = DataSetFifteen5().input;
            DataSetsFifteen[6] = DataSetFifteen6().input;
            DataSetsFifteen[7] = DataSetFifteen7().input;
            DataSetsFifteen[8] = DataSetFifteen8().input;
            DataSetsFifteen[9] = DataSetFifteen9().input;
            DataSetsFifteen[10] = DataSetFifteen10().input;
            DataSetsFifteen[11] = DataSetFifteen11().input;
            DataSetsFifteen[12] = DataSetFifteen12().input;
            DataSetsFifteen[13] = DataSetFifteen13().input;
            DataSetsFifteen[14] = DataSetFifteen14().input;
            DataSetsFifteen[15] = DataSetFifteen15().input;
            DataSetsFifteen[16] = DataSetFifteen16().input;
            DataSetsFifteen[17] = DataSetFifteen17().input;
            DataSetsFifteen[18] = DataSetFifteen18().input;
            DataSetsFifteen[19] = DataSetFifteen19().input;
            

        }

    }
}
