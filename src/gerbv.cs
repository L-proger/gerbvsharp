using System.Reflection.Metadata;

namespace Gerbvsharp
{
    public static partial class Gerbv {
        public const int APERTURE_MIN = 10;
        public const int APERTURE_MAX = 9999;

        /*
        * Maximum number of aperture parameters is set by the outline aperture macro.
        * There (p. 62) is defined up to 5000 points in outline. So 5000 points with x
        * and y plus outline shape code, exposure, # of vertices, and duplication of
        * start/end point gives
        */
        public const int APERTURE_PARAMETERS_MAX = 10006;
        public const int GERBV_SCALE_MIN         = 10;
        public const int GERBV_SCALE_MAX         = 40000;
        public const int MAX_ERRMSGLEN           = 25;
        public const int MAX_COORDLEN            = 28;
        public const int MAX_DISTLEN             = 180;
        public const int MAX_STATUSMSGLEN        = (MAX_ERRMSGLEN + MAX_COORDLEN + MAX_DISTLEN);

        /*
        * Files only have a limited precision in their data, so when interpreting
        * layer rotations or linear size that have been read from a project file, we
        * have to tolerate a certain amount of error.
        */
        public const double GERBV_PRECISION_ANGLE_RAD   = 1e-6;
        public const double GERBV_PRECISION_LINEAR_INCH = 1e-6;


        /* Convert between unscaled gerber coordinates and other units */
        /* XXX NOTE: Currently unscaled units are assumed as inch, this is not
        XXX necessarily true for all files */
        public static double COORD2INS(double c){ return c; }
        public static double COORD2MILS(double c){ return c * 1000.0; }
        public static double COORD2MMS(double c){ return c * 25.4; }


        public static double DEG2RAD(double d){ return d * Math.PI  / 180.0; }
        public static double RAD2DEG(double r){ return r * 180.0  / Math.PI; }
  

        public static void GERB_FATAL_ERROR(params object[] args) { /*g_log(NULL, G_LOG_LEVEL_ERROR, __VA_ARGS__)*/}
        public static void GERB_COMPILE_ERROR(params object[] args) { /*g_log(NULL, G_LOG_LEVEL_CRITICAL, __VA_ARGS__)*/}
        public static void GERB_COMPILE_WARNING(params object[] args) { /*g_log(NULL, G_LOG_LEVEL_WARNING, __VA_ARGS__)*/}
        public static void GERB_MESSAGE(params object[] args) { /*g_log(NULL, G_LOG_LEVEL_MESSAGE, __VA_ARGS__)*/}

        /*! The aperture macro commands */
        public enum gerbv_opcodes_t {
            GERBV_OPCODE_NOP,   /*!< no operation */
            GERBV_OPCODE_PUSH,  /*!< push the instruction onto the stack */
            GERBV_OPCODE_PPUSH, /*!< push parameter onto stack */
            GERBV_OPCODE_PPOP,  /*!< pop parameter from stack */
            GERBV_OPCODE_ADD,   /*!< mathmatical add operation */
            GERBV_OPCODE_SUB,   /*!< mathmatical subtract operation */
            GERBV_OPCODE_MUL,   /*!< mathmatical multiply operation */
            GERBV_OPCODE_DIV,   /*!< mathmatical divide operation */
            GERBV_OPCODE_PRIM   /*!< draw macro primative */
        };

        /*! The different message types used in libgerbv */
        public enum gerbv_message_type_t {
            GERBV_MESSAGE_FATAL,   /*!< processing cannot continue */
            GERBV_MESSAGE_ERROR,   /*!< something went wrong, but processing can still continue */
            GERBV_MESSAGE_WARNING, /*!< something was encountered that may provide the wrong output */
            GERBV_MESSAGE_NOTE     /*!< an irregularity was encountered, but needs no intervention */
        };

        /*! The different aperture types available.
        *  Please keep these in sync with the aperture names defined in
        *  gerbv_aperture_type_name() in gerbv.c */
        public enum gerbv_aperture_type_t {
            GERBV_APTYPE_NONE,          /*!< no aperture used */
            GERBV_APTYPE_CIRCLE,        /*!< a round aperture */
            GERBV_APTYPE_RECTANGLE,     /*!< a rectangular aperture */
            GERBV_APTYPE_OVAL,          /*!< an ovular (obround) aperture */
            GERBV_APTYPE_POLYGON,       /*!< a polygon aperture */
            GERBV_APTYPE_MACRO,         /*!< a RS274X macro */
            GERBV_APTYPE_MACRO_CIRCLE,  /*!< a RS274X circle macro */
            GERBV_APTYPE_MACRO_OUTLINE, /*!< a RS274X outline macro */
            GERBV_APTYPE_MACRO_POLYGON, /*!< a RS274X polygon macro */
            GERBV_APTYPE_MACRO_MOIRE,   /*!< a RS274X moire macro */
            GERBV_APTYPE_MACRO_THERMAL, /*!< a RS274X thermal macro */
            GERBV_APTYPE_MACRO_LINE20,  /*!< a RS274X vector line (code 20) macro */
            GERBV_APTYPE_MACRO_LINE21,  /*!< a RS274X centered line (code 21) macro */
            GERBV_APTYPE_MACRO_LINE22   /*!< a RS274X lower left line (code 22) macro */
        };

        /*! The current state of the aperture drawing tool */
        public enum gerbv_aperture_state_t {
            GERBV_APERTURE_STATE_OFF,  /*!< tool drawing is off, and nothing will be drawn */
            GERBV_APERTURE_STATE_ON,   /*!< tool drawing is on, and something will be drawn */
            GERBV_APERTURE_STATE_FLASH /*!< tool is flashing, and will draw a single aperture */
        };

        /*! The circle aperture macro parameter indexes */
        public enum gerbv_aptype_macro_circle_index_t {
            CIRCLE_EXPOSURE,
            CIRCLE_DIAMETER,
            CIRCLE_CENTER_X,
            CIRCLE_CENTER_Y,
        };

        public enum gerbv_aptype_macro_outline_index_t {
            OUTLINE_EXPOSURE,
            OUTLINE_NUMBER_OF_POINTS,
            OUTLINE_FIRST_X, /* x0 */
            OUTLINE_FIRST_Y, /* y0 */
            /* x1, y1, x2, y2, ..., rotation */
            OUTLINE_ROTATION, /* Rotation index is correct if outline has
                        no point except first */
        };

        /* Point number is from 0 (first) to (including) OUTLINE_NUMBER_OF_POINTS */
        public static int OUTLINE_X_IDX_OF_POINT(int number){ return 2 * number + (int)gerbv_aptype_macro_outline_index_t.OUTLINE_FIRST_X; }
        public static int OUTLINE_Y_IDX_OF_POINT(int number){ return 2 * number + (int)gerbv_aptype_macro_outline_index_t.OUTLINE_FIRST_Y; }
        public static int OUTLINE_ROTATION_IDX(int[] param_array){ return (int)param_array[(int)gerbv_aptype_macro_outline_index_t.OUTLINE_NUMBER_OF_POINTS] * 2 + (int)gerbv_aptype_macro_outline_index_t.OUTLINE_ROTATION; }

        public enum gerbv_aptype_macro_polygon_index_t {
            POLYGON_EXPOSURE,
            POLYGON_NUMBER_OF_POINTS,
            POLYGON_CENTER_X,
            POLYGON_CENTER_Y,
            POLYGON_DIAMETER,
            POLYGON_ROTATION,
        };

        public enum gerbv_aptype_macro_moire_index_t {
            MOIRE_CENTER_X,
            MOIRE_CENTER_Y,
            MOIRE_OUTSIDE_DIAMETER,
            MOIRE_CIRCLE_THICKNESS,
            MOIRE_GAP_WIDTH,
            MOIRE_NUMBER_OF_CIRCLES,
            MOIRE_CROSSHAIR_THICKNESS,
            MOIRE_CROSSHAIR_LENGTH,
            MOIRE_ROTATION,
        };

        public enum gerbv_aptype_macro_thermal_index_t {
            THERMAL_CENTER_X,
            THERMAL_CENTER_Y,
            THERMAL_OUTSIDE_DIAMETER,
            THERMAL_INSIDE_DIAMETER,
            THERMAL_CROSSHAIR_THICKNESS,
            THERMAL_ROTATION,
        };

        /*! The vector line aperture macro parameter indexes */
        public enum gerbv_aptype_macro_line20_index_t {
            LINE20_EXPOSURE,
            LINE20_LINE_WIDTH,
            LINE20_WIDTH = LINE20_LINE_WIDTH, /* Unification alias */
            LINE20_START_X,
            LINE20_START_Y,
            LINE20_END_X,
            LINE20_END_Y,
            LINE20_ROTATION,
        };

        /*! The centered line aperture macro parameter indexes */
        public enum gerbv_aptype_macro_line21_index_t {
            LINE21_EXPOSURE,
            LINE21_WIDTH,
            LINE21_HEIGHT,
            LINE21_CENTER_X,
            LINE21_CENTER_Y,
            LINE21_ROTATION,
        };

        /*! The lower left line aperture macro parameter indexes */
        public enum gerbv_aptype_macro_line22_index_t {
            LINE22_EXPOSURE,
            LINE22_WIDTH,
            LINE22_HEIGHT,
            LINE22_LOWER_LEFT_X,
            LINE22_LOWER_LEFT_Y,
            LINE22_ROTATION,
        };

        /*! The current unit used */
        public enum gerbv_unit_t {
            GERBV_UNIT_INCH,       /*!< inches */
            GERBV_UNIT_MM,         /*!< mm */
            GERBV_UNIT_UNSPECIFIED /*!< use default units */
        };

        /*! The different drawing polarities available */
        public enum gerbv_polarity_t {
            GERBV_POLARITY_POSITIVE, /*!< draw "positive", using the current layer's polarity */
            GERBV_POLARITY_NEGATIVE, /*!< draw "negative", reversing the current layer's polarity */
            GERBV_POLARITY_DARK,     /*!< add to the current rendering */
            GERBV_POLARITY_CLEAR     /*!< subtract from the current rendering */
        };

        /*! The decimal point parsing style used */
        public enum gerbv_omit_zeros_t {
            GERBV_OMIT_ZEROS_LEADING,    /*!< omit extra zeros before the decimal point */
            GERBV_OMIT_ZEROS_TRAILING,   /*!< omit extra zeros after the decimal point */
            GERBV_OMIT_ZEROS_EXPLICIT,   /*!< explicitly specify how many decimal places are used */
            GERBV_OMIT_ZEROS_UNSPECIFIED /*!< use the default parsing style */
        };

        /*! The coordinate system used */
        public enum gerbv_coordinate_t {
            GERBV_COORDINATE_ABSOLUTE,   /*!< all coordinates are absolute from a common origin */
            GERBV_COORDINATE_INCREMENTAL /*!< all coordinates are relative to the previous coordinate */
        };

        /*! The interpolation methods available.
        *  Please keep these in sync with the interpolation names defined in
        *  gerbv_interpolation_name() in gerbv.c */
        public enum gerbv_interpolation_t {
            GERBV_INTERPOLATION_LINEARx1,     /*!< draw a line */
            GERBV_INTERPOLATION_LINEARx10,    /*!< draw a line */
            GERBV_INTERPOLATION_LINEARx01,    /*!< draw a line */
            GERBV_INTERPOLATION_LINEARx001,   /*!< draw a line */
            GERBV_INTERPOLATION_CW_CIRCULAR,  /*!< draw an arc in the clockwise direction */
            GERBV_INTERPOLATION_CCW_CIRCULAR, /*!< draw an arc in the counter-clockwise direction */
            GERBV_INTERPOLATION_PAREA_START,  /*!< start a polygon draw */
            GERBV_INTERPOLATION_PAREA_END,    /*!< end a polygon draw */
            GERBV_INTERPOLATION_DELETED,       /*!< the net has been deleted by the user, and will not be drawn */
            GERBV_INTERPOLATION_x10 = GERBV_INTERPOLATION_LINEARx10 /* For backward compatibility */
        };

        public enum gerbv_encoding_t {
            GERBV_ENCODING_NONE,
            GERBV_ENCODING_ASCII,
            GERBV_ENCODING_EBCDIC,
            GERBV_ENCODING_BCD,
            GERBV_ENCODING_ISO_ASCII,
            GERBV_ENCODING_EIA
        };

        /*! The different layer types used */
        public enum gerbv_layertype_t {
            GERBV_LAYERTYPE_RS274X,           /*!< the file is a RS274X file */
            GERBV_LAYERTYPE_DRILL,            /*!< the file is an Excellon drill file */
            GERBV_LAYERTYPE_PICKANDPLACE_TOP, /*!< the file is a CSV pick and place file, top side */
            GERBV_LAYERTYPE_PICKANDPLACE_BOT, /*!< the file is a CSV pick and place file, bottom side */
        };

        public enum gerbv_knockout_type_t {
            GERBV_KNOCKOUT_TYPE_NOKNOCKOUT,
            GERBV_KNOCKOUT_TYPE_FIXEDKNOCK,
            GERBV_KNOCKOUT_TYPE_BORDER
        };

        public enum gerbv_mirror_state_t {
            GERBV_MIRROR_STATE_NOMIRROR,
            GERBV_MIRROR_STATE_FLIPA,
            GERBV_MIRROR_STATE_FLIPB,
            GERBV_MIRROR_STATE_FLIPAB
        };

        public enum gerbv_axis_select_t {
            GERBV_AXIS_SELECT_NOSELECT,
            GERBV_AXIS_SELECT_SWAPAB
        };

        public enum gerbv_image_justify_type_t {
            GERBV_JUSTIFY_NOJUSTIFY,
            GERBV_JUSTIFY_LOWERLEFT,
            GERBV_JUSTIFY_CENTERJUSTIFY
        };


        /*! A linked list of errors found in the files */
        public class gerbv_error_list_t {
            public int                  layer;
            public string?              error_text;
            public gerbv_message_type_t type;
            public gerbv_error_list_t?  next;
        };

        public class gerbv_instruction_t {
            public gerbv_opcodes_t opcode;

            public struct Data {
                public int   ival;
                public float fval;
            };
            public Data data;
            public gerbv_instruction_t? next;
        };

        public class gerbv_amacro_t {
            public string               name;
            public gerbv_instruction_t  program;
            public uint                 nuf_push; /* Nuf pushes in program to estimate stack size */
            public gerbv_amacro_t       next;
        };
    }
}  