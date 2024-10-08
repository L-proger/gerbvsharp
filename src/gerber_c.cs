using System;
using static Gerbvsharp.Gerbv;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gerbvsharp {
    public static partial class Gerbv {

        /* --------------------------------------------------------- */
        public static gerbv_net_t gerber_create_new_net(gerbv_net_t currentNet, gerbv_layer_t layer, gerbv_netstate_t state) {
            gerbv_net_t newNet = new gerbv_net_t();

            currentNet.next = newNet;
            if (layer != null)
                newNet.layer = layer;
            else
                newNet.layer = currentNet.layer;
            if (state != null)
                newNet.state = state;
            else
                newNet.state = currentNet.state;
            return newNet;
        }

        /* --------------------------------------------------------- */
        public static bool gerber_create_new_aperture(
            gerbv_image_t image, ref int indexNumber, gerbv_aperture_type_t apertureType, double parameter1, double parameter2
        ) {
            int i;

            /* search for an available aperture spot */
            for (i = 0; i <= APERTURE_MAX; i++) {
                if (image.aperture[i] == null) {
                    image.aperture[i] = new gerbv_aperture_t();
                    image.aperture[i].type = apertureType;
                    image.aperture[i].parameter[0] = parameter1;
                    image.aperture[i].parameter[1] = parameter2;
                    indexNumber = i;
                    return true;
                }
            }
            return false;
        }


        /* --------------------------------------------------------- */
        /*! This function reads the Gerber file char by char, looking
         *  for various Gerber codes (e.g. G, D, etc).  Once it reads
         *  a code, it then dispatches control to one or another
         *  bits of code which parse the individual code.
         *  It also updates the state struct, which holds info about
         *  the current state of the hypothetical photoplotter
         *  (i.e. updates whether the aperture is on or off, updates
         *  any other parameters, like units, offsets, apertures, etc.)
         */
        //public static bool gerber_parse_file_segment(int levelOfRecursion, gerbv_image_t image, gerb_state_t state, gerbv_net_t curr_net, gerbv_stats_t stats, gerb_file_t fd, string directoryPath) {
        //    int read, coord, len, polygonPoints = 0;
        //    double x_scale = 0.0, y_scale = 0.0;
        //    double delta_cp_x = 0.0, delta_cp_y = 0.0;
        //    double aperture_sizeX, aperture_sizeY;
        //    double scale;
        //    bool foundEOF = false;
        //    gerbv_render_size_t boundingBoxNew = new gerbv_render_size_t { left = double.PositiveInfinity, right = double.NegativeInfinity, bottom = double.PositiveInfinity, top = double.NegativeInfinity }, boundingBox = boundingBoxNew;
        //    gerbv_error_list_t error_list = stats.error_list;
        //    long line_num = 1;
        //
        //    char EOF = char.MaxValue;
        //
        //    while ((read = gerb_fgetc(fd)) != EOF) {
        //        /* figure out the scale, since we need to normalize
        //       all dimensions to inches */
        //        if (state.state.unit == gerbv_unit_t.GERBV_UNIT_MM)
        //            scale = 25.4;
        //        else
        //            scale = 1.0;
        //        switch ((char)(read & 0xff)) {
        //            case 'G':
        //                //dprintf("... Found G code at line %ld\n", line_num);
        //                parse_G_code(fd, state, image, line_num);
        //                break;
        //            case 'D':
        //                //dprintf("... Found D code at line %ld\n", line_num);
        //                parse_D_code(fd, state, image, line_num);
        //                break;
        //            case 'M':
        //                //dprintf("... Found M code at line %ld\n", line_num);
        //
        //                switch (parse_M_code(fd, image, line_num)) {
        //                    case 1:
        //                    case 2:
        //                    case 3: foundEOF = true; break;
        //                    default:
        //                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unknown M code found at line {line_num} in file \"{fd.filename}\"");
        //                        break;
        //                } /* switch(parse_M_code) */
        //                break;
        //            case 'X':
        //                stats.X++;
        //                coord = gerb_fgetint(fd, out len);
        //                if (image.format != null)
        //                    add_trailing_zeros_if_omitted(
        //                        &coord, image.format.x_int + image.format.x_dec - len, image.format
        //                    );
        //                //dprintf("... Found X code %d at line %ld\n", coord, line_num);
        //                if ((image.format != null) && (image.format.coordinate == gerbv_coordinate_t.GERBV_COORDINATE_INCREMENTAL))
        //                    state.curr_x += coord;
        //                else
        //                    state.curr_x = coord;
        //
        //                state.changed = 1;
        //                break;
        //
        //            case 'Y':
        //                stats.Y++;
        //                coord = gerb_fgetint(fd, out len);
        //                if (image.format != null)
        //                    add_trailing_zeros_if_omitted(
        //                        &coord, image.format.y_int + image.format.y_dec - len, image.format
        //                    );
        //                //dprintf("... Found Y code %d at line %ld\n", coord, line_num);
        //                if (image.format && image.format.coordinate == GERBV_COORDINATE_INCREMENTAL)
        //                    state.curr_y += coord;
        //                else
        //                    state.curr_y = coord;
        //
        //                state.changed = 1;
        //                break;
        //
        //            case 'I':
        //                stats.I++;
        //                coord = gerb_fgetint(fd, out len);
        //                if (image.format != null)
        //                    add_trailing_zeros_if_omitted(
        //                        &coord, image.format.x_int + image.format.x_dec - len, image.format
        //                    );
        //                //dprintf("... Found I code %d at line %ld\n", coord, line_num);
        //                state.delta_cp_x = coord;
        //                state.changed = 1;
        //                break;
        //
        //            case 'J':
        //                stats.J++;
        //                coord = gerb_fgetint(fd, out len);
        //                if (image.format != null)
        //                    add_trailing_zeros_if_omitted(
        //                        &coord, image.format.y_int + image.format.y_dec - len, image.format
        //                    );
        //                //dprintf("... Found J code %d at line %ld\n", coord, line_num);
        //                state.delta_cp_y = coord;
        //                state.changed = 1;
        //                break;
        //
        //            case '%':
        //                //dprintf("... Found %% code at line %ld\n", line_num);
        //                while (true) {
        //                    parse_rs274x(levelOfRecursion, fd, image, state, curr_net, stats, directoryPath, &line_num);
        //
        //                    /* advance past any whitespace here */
        //                    int c;
        //                    while (1) {
        //                        c = gerb_fgetc(fd);
        //
        //                        switch (c) {
        //                            case '\0':
        //                            case '\t':
        //                            case ' ': continue;
        //
        //                            case '\n':
        //                                line_num++;
        //
        //                                /* Get <CR> char, if any, from <LF><CR> pair */
        //                                read = gerb_fgetc(fd);
        //                                if (read != '\r' && read != EOF)
        //                                    gerb_ungetc(fd);
        //
        //                                continue;
        //
        //                            case '\r':
        //                                line_num++;
        //
        //                                /* Get <LF> char, if any, from <CR><LF> pair */
        //                                read = gerb_fgetc(fd);
        //                                if (read != '\n' && read != EOF)
        //                                    gerb_ungetc(fd);
        //
        //                                continue;
        //                        }
        //
        //                        break; /* break while(1) */
        //                    };
        //
        //                    if (c == EOF || c == '%')
        //                        break;
        //
        //                    /* Loop again to catch multiple blocks on the same line
        //                     * (separated by * char) */
        //                    gerb_ungetc(fd);
        //                }
        //                break;
        //            case '*':
        //                //dprintf("... Found * code at line %ld\n", line_num);
        //                stats.star++;
        //                if (state.changed == 0)
        //                    break;
        //                state.changed = 0;
        //
        //                /* don't even bother saving the net if the aperture state is GERBV_APERTURE_STATE_OFF and we
        //                   aren't starting a polygon fill (where we need it to get to the start point) */
        //                if ((state.aperture_state == GERBV_APERTURE_STATE_OFF) && (state.in_parea_fill == 0)
        //                    && (state.interpolation != GERBV_INTERPOLATION_PAREA_START)) {
        //                    /* save the coordinate so the next net can use it for a start point */
        //                    state.prev_x = state.curr_x;
        //                    state.prev_y = state.curr_y;
        //                    break;
        //                }
        //                curr_net = gerber_create_new_net(curr_net, state.layer, state.state);
        //                /*
        //                 * Scale to given coordinate format
        //                 * XXX only "omit leading zeros".
        //                 */
        //                if (image && image.format) {
        //                    x_scale = pow(10.0, (double)image.format.x_dec);
        //                    y_scale = pow(10.0, (double)image.format.y_dec);
        //                }
        //                x_scale *= scale;
        //                y_scale *= scale;
        //                curr_ne.start_x = (double)state.prev_x / x_scale;
        //                curr_ne.start_y = (double)state.prev_y / y_scale;
        //                curr_ne.stop_x  = (double)state.curr_x / x_scale;
        //                curr_ne.stop_y  = (double)state.curr_y / y_scale;
        //                delta_cp_x = (double)state.delta_cp_x / x_scale;
        //                delta_cp_y = (double)state.delta_cp_y / y_scale;
        //
        //                switch (state.interpolation) {
        //                    case gerbv_interpolation_t.GERBV_INTERPOLATION_CW_CIRCULAR:
        //                    case gerbv_interpolation_t.GERBV_INTERPOLATION_CCW_CIRCULAR: {
        //                            int cw = (state.interpolation == gerbv_interpolation_t.GERBV_INTERPOLATION_CW_CIRCULAR) ? 1 : 0;
        //
        //                            curr_net.cirseg = g_new0(gerbv_cirseg_t, 1);
        //                            if (state.mq_on) {
        //                                calc_cirseg_mq(curr_net, cw, delta_cp_x, delta_cp_y);
        //                            } else {
        //                                calc_cirseg_sq(curr_net, cw, delta_cp_x, delta_cp_y);
        //
        //                                /*
        //                                 * In single quadrant circular interpolation Ix and Jy
        //                                 * incremental distance must be unsigned.
        //                                 */
        //                                if (delta_cp_x < 0 || delta_cp_y < 0) {
        //                                    gerbv_stats_printf(
        //                                        error_list, GERBV_MESSAGE_ERROR, -1,
        //                                        _("Signed incremental distance IxJy "
        //
        //                                          "in single quadrant %s circular "
        //
        //                                          "interpolation %s at line %ld "
        //
        //                                          "in file \"%s\""),
        //                                        cw ? _("CW") : _("CCW"), cw ? "G02" : "G03", line_num, fd.filename
        //                                    );
        //                                }
        //                            }
        //                            break;
        //                        }
        //                    case gerbv_interpolation_t.GERBV_INTERPOLATION_PAREA_START:
        //                        /*
        //                         * To be able to get back and fill in number of polygon corners
        //                         */
        //                        state.parea_start_node = curr_net;
        //                        state.in_parea_fill = 1;
        //                        polygonPoints = 0;
        //                        boundingBox = boundingBoxNew;
        //                        break;
        //                    case gerbv_interpolation_t.GERBV_INTERPOLATION_PAREA_END:
        //                        /* save the calculated bounding box to the master node */
        //                        if (state.parea_start_node != null) {
        //                            state.parea_start_node.boundingBox = boundingBox;
        //                        } else {
        //                            gerbv_stats_printf(
        //                                error_list, GERBV_MESSAGE_ERROR, -1,
        //                                _("End of polygon without start "
        //
        //                                  "at line %ld in file \"%s\""),
        //                                line_num, fd.filename
        //                            );
        //                        }
        //
        //                        /* close out the polygon */
        //                        state.parea_start_node = null;
        //                        state.in_parea_fill = 0;
        //                        polygonPoints = 0;
        //                        break;
        //                    default: break;
        //                } /* switch(state.interpolation) */
        //
        //                /*
        //                 * Count number of points in Polygon Area
        //                 */
        //                if (state.in_parea_fill && state.parea_start_node) {
        //                    /*
        //                     * "...all lines drawn with D01 are considered edges of the
        //                     * polygon. D02 closes and fills the polygon."
        //                     * p.49 rs274xrevd_e.pdf
        //                     * D02 . state.aperture_state == GERBV_APERTURE_STATE_OFF
        //                     */
        //
        //                    /* UPDATE: only end the polygon during a D02 call if we've already
        //                       drawn a polygon edge (with D01) */
        //
        //                    if (state.aperture_state == GERBV_APERTURE_STATE_OFF
        //                        && state.interpolation != GERBV_INTERPOLATION_PAREA_START && polygonPoints > 0) {
        //                        curr_net.interpolation = GERBV_INTERPOLATION_PAREA_END;
        //                        curr_net = gerber_create_new_net(curr_net, state.layer, state.state);
        //                        curr_net.interpolation = GERBV_INTERPOLATION_PAREA_START;
        //                        state.parea_start_node.boundingBox = boundingBox;
        //                        state.parea_start_node = curr_net;
        //                        polygonPoints = 0;
        //                        curr_net = gerber_create_new_net(curr_net, state.layer, state.state);
        //                        curr_net.start_x = (double)state.prev_x / x_scale;
        //                        curr_net.start_y = (double)state.prev_y / y_scale;
        //                        curr_net.stop_x = (double)state.curr_x / x_scale;
        //                        curr_net.stop_y = (double)state.curr_y / y_scale;
        //                        boundingBox = boundingBoxNew;
        //                    } else if (state.interpolation != GERBV_INTERPOLATION_PAREA_START)
        //                        polygonPoints++;
        //
        //                } /* if (state.in_parea_fill && state.parea_start_node) */
        //
        //                curr_net.interpolation = state.interpolation;
        //
        //                /*
        //                 * Override circular interpolation if no center was given.
        //                 * This should be a safe hack, since a good file should always
        //                 * include I or J.  And even if the radius is zero, the endpoint
        //                 * should be the same as the start point, creating no line
        //                 */
        //                if (((state.interpolation == GERBV_INTERPOLATION_CW_CIRCULAR)
        //                     || (state.interpolation == GERBV_INTERPOLATION_CCW_CIRCULAR))
        //                    && ((state.delta_cp_x == 0.0) && (state.delta_cp_y == 0.0)))
        //                    curr_net.interpolation = GERBV_INTERPOLATION_LINEARx1;
        //
        //                /*
        //                 * If we detected the end of Polygon Area Fill we go back to
        //                 * the interpolation we had before that.
        //                 * Also if we detected any of the quadrant flags, since some
        //                 * gerbers don't reset the interpolation (EagleCad again).
        //                 */
        //                if ((state.interpolation == GERBV_INTERPOLATION_PAREA_START
        //                     || state.interpolation == GERBV_INTERPOLATION_PAREA_END)
        //                    && state.prev_interpolation != GERBV_INTERPOLATION_PAREA_END) {
        //                    state.interpolation = state.prev_interpolation;
        //                }
        //
        //                /*
        //                 * Save layer polarity and unit
        //                 */
        //                curr_net.layer = state.layer;
        //
        //                state.delta_cp_x = 0.0;
        //                state.delta_cp_y = 0.0;
        //                curr_net.aperture = state.curr_aperture;
        //                curr_net.aperture_state = state.aperture_state;
        //
        //                /*
        //                 * For next round we save the current position as
        //                 * the previous position
        //                 */
        //                state.prev_x = state.curr_x;
        //                state.prev_y = state.curr_y;
        //
        //                /*
        //                 * If we have an aperture defined at the moment we find
        //                 * min and max of image with compensation for mm.
        //                 */
        //                if ((curr_net.aperture == 0) && !state.in_parea_fill)
        //                    break;
        //
        //                /* only update the min/max values and aperture stats if we are drawing */
        //                if ((curr_net.aperture_state != GERBV_APERTURE_STATE_OFF)
        //                    && (curr_net.interpolation != GERBV_INTERPOLATION_PAREA_START)) {
        //                    double repeat_off_X = 0.0, repeat_off_Y = 0.0;
        //
        //                    /* Update stats with current aperture number if not in polygon */
        //                    if (!state.in_parea_fill) {
        //                        dprintf("     In %s(), adding 1 to D_list ...\n", __func__);
        //                        int retcode =
        //                            gerbv_stats_increment_D_list_count(stats.D_code_list, curr_net.aperture, 1, error_list);
        //                        if (retcode == -1) {
        //                            gerbv_stats_printf(
        //                                error_list, GERBV_MESSAGE_ERROR, -1,
        //                                _("Found undefined D code D%02d "
        //
        //                                  "at line %ld in file \"%s\""),
        //                                curr_net.aperture, line_num, fd.filename
        //                            );
        //                            stats.D_unknown++;
        //                        }
        //                    }
        //
        //                    /*
        //                     * If step_and_repeat (%SR%) is used, check min_x,max_y etc for
        //                     * the ends of the step_and_repeat lattice. This goes wrong in
        //                     * the case of negative dist_X or dist_Y, in which case we
        //                     * should compare against the startpoints of the lines, not
        //                     * the stoppoints, but that seems an uncommon case (and the
        //                     * error isn't very big any way).
        //                     */
        //                    repeat_off_X = (state.layer.stepAndRepeat.X - 1) * state.layer.stepAndRepeat.dist_X;
        //                    repeat_off_Y = (state.layer.stepAndRepeat.Y - 1) * state.layer.stepAndRepeat.dist_Y;
        //
        //                    cairo_matrix_init(&currentMatrix, 1, 0, 0, 1, 0, 0);
        //                    /* offset image */
        //                    cairo_matrix_translate(&currentMatrix, image.info.offsetA, image.info.offsetB);
        //                    /* do image rotation */
        //                    cairo_matrix_rotate(&currentMatrix, image.info.imageRotation);
        //                    /* it's a new layer, so recalculate the new transformation
        //                     * matrix for it */
        //                    /* do any rotations */
        //                    cairo_matrix_rotate(&currentMatrix, state.layer.rotation);
        //
        //                    /* calculate current layer and state transformation matrices */
        //                    /* apply scale factor */
        //                    cairo_matrix_scale(&currentMatrix, state.state.scaleA, state.state.scaleB);
        //                    /* apply offset */
        //                    cairo_matrix_translate(&currentMatrix, state.state.offsetA, state.state.offsetB);
        //                    /* apply mirror */
        //                    switch (state.state.mirrorState) {
        //                        case GERBV_MIRROR_STATE_FLIPA: cairo_matrix_scale(&currentMatrix, -1, 1); break;
        //                        case GERBV_MIRROR_STATE_FLIPB: cairo_matrix_scale(&currentMatrix, 1, -1); break;
        //                        case GERBV_MIRROR_STATE_FLIPAB: cairo_matrix_scale(&currentMatrix, -1, -1); break;
        //                        default: break;
        //                    }
        //                    /* finally, apply axis select */
        //                    if (state.state.axisSelect == GERBV_AXIS_SELECT_SWAPAB) {
        //                        /* we do this by rotating 270 (counterclockwise, then
        //                         *  mirroring the Y axis
        //                         */
        //                        cairo_matrix_rotate(&currentMatrix, M_PI + M_PI_2);
        //                        cairo_matrix_scale(&currentMatrix, 1, -1);
        //                    }
        //                    /* if it's a macro, step through all the primitive components
        //                       and calculate the true bounding box */
        //                    if ((image.aperture[curr_net.aperture] != null)
        //                        && (image.aperture[curr_net.aperture].type == GERBV_APTYPE_MACRO)) {
        //                        gerbv_simplified_amacro_t ls = image.aperture[curr_net.aperture].simplified;
        //
        //                        while (ls != null) {
        //                            double offsetx = 0, offsety = 0, widthx = 0, widthy = 0;
        //                            bool calculatedAlready = false;
        //
        //                            if (ls.type == GERBV_APTYPE_MACRO_CIRCLE) {
        //                                offsetx = ls.parameter[CIRCLE_CENTER_X];
        //                                offsety = ls.parameter[CIRCLE_CENTER_Y];
        //                                widthx = widthy = ls.parameter[CIRCLE_DIAMETER];
        //                            } else if (ls.type == GERBV_APTYPE_MACRO_OUTLINE) {
        //                                int pointCounter, numberOfPoints;
        //                                numberOfPoints = ls.parameter[OUTLINE_NUMBER_OF_POINTS] + 1;
        //
        //                                for (pointCounter = 0; pointCounter < numberOfPoints; pointCounter++) {
        //                                    gerber_update_min_and_max(
        //                                        &boundingBox,
        //                                        curr_net.stop_x + ls.parameter[OUTLINE_X_IDX_OF_POINT(pointCounter)],
        //                                        curr_net.stop_y + ls.parameter[OUTLINE_Y_IDX_OF_POINT(pointCounter)], 0, 0, 0,
        //                                        0
        //                                    );
        //                                }
        //                                calculatedAlready = TRUE;
        //                            } else if (ls.type == GERBV_APTYPE_MACRO_POLYGON) {
        //                                offsetx = ls.parameter[POLYGON_CENTER_X];
        //                                offsety = ls.parameter[POLYGON_CENTER_Y];
        //                                widthx = widthy = ls�parameter[POLYGON_DIAMETER];
        //                            } else if (ls.type == GERBV_APTYPE_MACRO_MOIRE) {
        //                                offsetx = ls.parameter[MOIRE_CENTER_X];
        //                                offsety = ls.parameter[MOIRE_CENTER_Y];
        //                                widthx = widthy = ls.parameter[MOIRE_OUTSIDE_DIAMETER];
        //                            } else if (ls.type == GERBV_APTYPE_MACRO_THERMAL) {
        //                                offsetx = ls.parameter[THERMAL_CENTER_X];
        //                                offsety = ls.parameter[THERMAL_CENTER_Y];
        //                                widthx = widthy = ls.parameter[THERMAL_OUTSIDE_DIAMETER];
        //                            } else if (ls.type == GERBV_APTYPE_MACRO_LINE20) {
        //                                widthx = widthy = ls.parameter[LINE20_LINE_WIDTH];
        //                                gerber_update_min_and_max(
        //                                    &boundingBox, curr_net.stop_x + ls.parameter[LINE20_START_X],
        //                                    curr_net.stop_y + ls.parameter[LINE20_START_Y], widthx / 2, widthx / 2,
        //                                    widthy / 2, widthy / 2
        //                                );
        //                                gerber_update_min_and_max(
        //                                    &boundingBox, curr_net.stop_x + ls.parameter[LINE20_END_X],
        //                                    curr_net.stop_y + ls.parameter[LINE20_END_Y], widthx / 2, widthx / 2, widthy / 2,
        //                                    widthy / 2
        //                                );
        //                                calculatedAlready = TRUE;
        //                            } else if (ls.type == GERBV_APTYPE_MACRO_LINE21) {
        //                                double largestDimension = hypot(ls.parameter[LINE21_WIDTH], ls.parameter[LINE21_HEIGHT]);
        //                                offsetx = ls.parameter[LINE21_CENTER_X];
        //                                offsety = ls.parameter[LINE21_CENTER_Y];
        //                                widthx = widthy = largestDimension;
        //                            } else if (ls.type == GERBV_APTYPE_MACRO_LINE22) {
        //                                double largestDimension = hypot(ls.parameter[LINE22_WIDTH], ls.parameter[LINE22_HEIGHT]);
        //
        //                                offsetx = ls.parameter[LINE22_LOWER_LEFT_X] + ls.parameter[LINE22_WIDTH] / 2;
        //                                offsety = ls.parameter[LINE22_LOWER_LEFT_Y] + ls.parameter[LINE22_HEIGHT] / 2;
        //                                widthx = widthy = largestDimension;
        //                            }
        //
        //                            if (!calculatedAlready) {
        //                                gerber_update_min_and_max(
        //                                    &boundingBox, curr_net.stop_x + offsetx, curr_net.stop_y + offsety, widthx / 2,
        //                                    widthx / 2, widthy / 2, widthy / 2
        //                                );
        //                            }
        //                            ls = ls.next;
        //                        }
        //                    } else {
        //                        if (image.aperture[curr_net.aperture] != null) {
        //                            aperture_sizeX = image.aperture[curr_net.aperture].parameter[0];
        //                            if ((image.aperture[curr_net.aperture].type == GERBV_APTYPE_RECTANGLE)
        //                                || (image.aperture[curr_net.aperture].type == GERBV_APTYPE_OVAL)) {
        //                                aperture_sizeY = image.aperture[curr_net.aperture].parameter[1];
        //                            } else
        //                                aperture_sizeY = aperture_sizeX;
        //                        } else {
        //                            /* this is usually for polygon fills, where the aperture width
        //                               is "zero" */
        //                            aperture_sizeX = aperture_sizeY = 0;
        //                        }
        //
        //                        /* if it's an arc path, use a special calc */
        //
        //                        if ((curr_net.interpolation == GERBV_INTERPOLATION_CW_CIRCULAR)
        //                            || (curr_net.interpolation == GERBV_INTERPOLATION_CCW_CIRCULAR)) {
        //                            calc_cirseg_bbox(curr_net.cirseg, aperture_sizeX, aperture_sizeY, &boundingBox);
        //                        } else {
        //                            /* check both the start and stop of the aperture points against
        //                               a running min/max counter */
        //                            /* Note: only check start coordinate if this isn't a flash,
        //                               since the start point may be bogus if it is a flash */
        //                            if (curr_net.aperture_state != GERBV_APERTURE_STATE_FLASH) {
        //                                gerber_update_min_and_max(
        //                                    &boundingBox, curr_net.start_x, curr_net.start_y, aperture_sizeX / 2,
        //                                    aperture_sizeX / 2, aperture_sizeY / 2, aperture_sizeY / 2
        //                                );
        //                            }
        //                            gerber_update_min_and_max(
        //                                &boundingBox, curr_net.stop_x, curr_net.stop_y, aperture_sizeX / 2,
        //                                aperture_sizeX / 2, aperture_sizeY / 2, aperture_sizeY / 2
        //                            );
        //                        }
        //                    }
        //                    /* update the info bounding box with this latest bounding box */
        //                    /* don't change the bounding box if the polarity is clear */
        //                    if (state.layer.polarity != GERBV_POLARITY_CLEAR) {
        //                        gerber_update_image_min_max(&boundingBox, repeat_off_X, repeat_off_Y, image);
        //                    }
        //                    /* optionally update the knockout measurement box */
        //                    if (knockoutMeasure) {
        //                        if (boundingBox.left < knockoutLimitXmin)
        //                            knockoutLimitXmin = boundingBox.left;
        //                        if (boundingBox.right + repeat_off_X > knockoutLimitXmax)
        //                            knockoutLimitXmax = boundingBox.right + repeat_off_X;
        //                        if (boundingBox.bottom < knockoutLimitYmin)
        //                            knockoutLimitYmin = boundingBox.bottom;
        //                        if (boundingBox.top + repeat_off_Y > knockoutLimitYmax)
        //                            knockoutLimitYmax = boundingBox.top + repeat_off_Y;
        //                    }
        //                    /* if we're not in a polygon fill, then update the object bounding box */
        //                    if (!state.in_parea_fill) {
        //                        curr_net.boundingBox = boundingBox;
        //                        boundingBox = boundingBoxNew;
        //                    }
        //                }
        //                break;
        //
        //            case '\0':
        //            case '\t':
        //            case ' ': break;
        //
        //            case '\n':
        //                line_num++;
        //
        //                /* Get <CR> char, if any, from <LF><CR> pair */
        //                read = gerb_fgetc(fd);
        //                if (read != '\r' && read != EOF)
        //                    gerb_ungetc(fd);
        //                break;
        //
        //            case '\r':
        //                line_num++;
        //
        //                /* Get <LF> char, if any, from <CR><LF> pair */
        //                read = gerb_fgetc(fd);
        //                if (read != '\n' && read != EOF)
        //                    gerb_ungetc(fd);
        //                break;
        //
        //            default:
        //                stats.unknown++;
        //                gerbv_stats_printf(
        //                    error_list, GERBV_MESSAGE_ERROR, -1,
        //                    _("Found unknown character '%s' (0x%x) "
        //
        //                      "at line %ld in file \"%s\""),
        //                    gerbv_escape_char(read), read, line_num, fd.filename
        //                );
        //        } /* switch((char) (read & 0xff)) */
        //    }
        //    return foundEOF;
        //}



        /* ------------------------------------------------------------------ */
        /*! This is a wrapper which gets called from top level.  It
         *  does some initialization and pre-processing, and
         *  then calls gerber_parse_file_segment
         *  which processes the actual file.  Then it does final
         *  modifications to the image created.
         */

        public static partial gerbv_image_t parse_gerb(gerb_file_t fd, string directoryPath) {
            gerb_state_t? state = null;
            gerbv_image_t? image = null;
            gerbv_net_t? curr_net = null;
            gerbv_stats_t? stats;
            bool foundEOF = false;

            //throw new NotImplementedException();

            /*
             * Create new state.  This is used locally to keep track
             * of the photoplotter's state as the Gerber is read in.
             */
            state = new gerb_state_t();
            
            /*
             * Create new image.  This will be returned.
             */
            image = gerbv_create_image(image, "RS274-X (Gerber) File");
            if (image == null) {
                throw new Exception("malloc image failed");
            }
            curr_net = image.netlist;
            image.layertype = gerbv_layertype_t.GERBV_LAYERTYPE_RS274X;
            image.gerbv_stats = gerbv_stats_new();
            if (image.gerbv_stats == null) {
                throw new Exception("malloc gerbv_stats failed in %s()");
            }
                
            
            stats = image.gerbv_stats;
            
            /* set active layer and netstate to point to first default one created */
            state.layer = image.layers[0];
            state.state = image.states[0];
            curr_net.layer = state.layer;
            curr_net.state = state.state;
            
            /*
             * Start parsing
             */
            //Console.WriteLine("In %s(), starting to parse file...");
            //foundEOF = gerber_parse_file_segment(1, image, state, curr_net, stats, fd, directoryPath);
            //
            //if (!foundEOF) {
            //    gerbv_stats_printf(
            //        stats.error_list, GERBV_MESSAGE_ERROR, -1, _("Missing Gerber EOF code in file \"%s\""), fd.filename
            //    );
            //}
            //
            //
            //Console.WriteLine("               ... done parsing Gerber file");
            //gerber_update_any_running_knockout_measurements(image);
            //gerber_calculate_final_justify_effects(image);
            
            return image;
        } /* parse_gerb */

        /* ------------------------------------------------------------------- */
        /*! This function reads a G number and updates the current
         *  state.  It also updates the G stats counters
         */
        public static void parse_G_code(gerb_file_t fd, gerb_state_t state, gerbv_image_t image, long line_num_p) {
            int op_int;
            gerbv_format_t format = image.format;
            gerbv_stats_t stats = image.gerbv_stats;
            gerbv_error_list_t error_list = stats.error_list;
            int c;
            int getintlen = 0;
            char EOF = char.MaxValue;
            op_int = gerb_fgetint(fd, out getintlen);

            

            /* Emphasize text with new line '\n' in the beginning */
            //dprintf("\n     Found G%02d at line %ld (%s)\n", op_int, *line_num_p, gerber_g_code_name(op_int));

            switch (op_int) {
                case 0: /* Move */
                    /* Is this doing anything really? */
                    stats.G0++;
                    break;
                case 1: /* Linear Interpolation (1X scale) */
                    state.interpolation = gerbv_interpolation_t.GERBV_INTERPOLATION_LINEARx1;
                    stats.G1++;
                    break;
                case 2: /* Clockwise Linear Interpolation */
                    state.interpolation = gerbv_interpolation_t.GERBV_INTERPOLATION_CW_CIRCULAR;
                    stats.G2++;
                    break;
                case 3: /* Counter Clockwise Linear Interpolation */
                    state.interpolation = gerbv_interpolation_t.GERBV_INTERPOLATION_CCW_CIRCULAR;
                    stats.G3++;
                    break;
                case 4: /* Ignore Data Block */
                    /* Don't do anything, just read 'til * */
                    do {
                        c = gerb_fgetc(fd);
                        if (c == '\r' || c == '\n') {
                            gerbv_stats_printf(
                                error_list, gerbv_message_type_t.GERBV_MESSAGE_WARNING, -1,
                                $"Found newline while parsing G04 code at line {line_num_p} in file \"{fd.filename}\", maybe you forgot a \"*\"?");
                        }
                    } while (c != EOF && c != '*');

                    stats.G4++;
                    break;
                case 10: /* Linear Interpolation (10X scale) */
                    state.interpolation = gerbv_interpolation_t.GERBV_INTERPOLATION_LINEARx10;
                    stats.G10++;
                    break;
                case 11: /* Linear Interpolation (0.1X scale) */
                    state.interpolation = gerbv_interpolation_t.GERBV_INTERPOLATION_LINEARx01;
                    stats.G11++;
                    break;
                case 12: /* Linear Interpolation (0.01X scale) */
                    state.interpolation = gerbv_interpolation_t.GERBV_INTERPOLATION_LINEARx001;
                    stats.G12++;
                    break;
                case 36: /* Turn on Polygon Area Fill */
                    state.prev_interpolation = state.interpolation;
                    state.interpolation = gerbv_interpolation_t.GERBV_INTERPOLATION_PAREA_START;
                    state.changed = 1;
                    stats.G36++;
                    break;
                case 37: /* Turn off Polygon Area Fill */
                    state.interpolation = gerbv_interpolation_t.GERBV_INTERPOLATION_PAREA_END;
                    state.changed = 1;
                    stats.G37++;
                    break;
                case 54: /* Tool prepare */
                    /* XXX Maybe uneccesary??? */
                    if (gerb_fgetc(fd) == 'D') {
                        int a = gerb_fgetint(fd, out getintlen);
                        if ((a >= 0) && (a <= APERTURE_MAX)) {
                            state.curr_aperture = a;
                        } else {
                            gerbv_stats_printf(
                                error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1,
                                $"Found aperture D{a} out of bounds while parsing G code at line {line_num_p} in file \"{fd.filename}\"");
                        }
                    } else {
                        gerbv_stats_printf(
                            error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1,
                            $"Found unexpected code after G54 at line {line_num_p} in file \"{fd.filename}\"");
                        /* TODO: insert error count here */
                    }
                    stats.G54++;
                    break;
                case 55: /* Prepare for flash */ stats.G55++; break;
                case 70: /* Specify inches */
                    state.state = gerbv_image_return_new_netstate(state.state);
                    state.state.unit = gerbv_unit_t.GERBV_UNIT_INCH;
                    stats.G70++;
                    break;
                case 71: /* Specify millimeters */
                    state.state = gerbv_image_return_new_netstate(state.state);
                    state.state.unit = gerbv_unit_t.GERBV_UNIT_MM;
                    stats.G71++;
                    break;
                case 74: /* Disable 360 circular interpolation */
                    state.mq_on = 0;
                    stats.G74++;
                    break;
                case 75: /* Enable 360 circular interpolation */
                    state.mq_on = 1;
                    stats.G75++;
                    break;
                case 90: /* Specify absolut format */
                    if (format != null)
                        format.coordinate = gerbv_coordinate_t.GERBV_COORDINATE_ABSOLUTE;
                    stats.G90++;
                    break;
                case 91: /* Specify incremental format */
                    if (format != null)
                        format.coordinate = gerbv_coordinate_t.GERBV_COORDINATE_INCREMENTAL;
                    stats.G91++;
                    break;
                default:
                    gerbv_stats_printf(
                        error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1,
                        $"Encountered unknown G code G{op_int} at line {line_num_p} in file \"{fd.filename}\"");
                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_WARNING, -1, $"Ignoring unknown G code G{op_int}");
                    stats.G_unknown++;
                    /* TODO: insert error count here */

                    break;
            }

            return;
        } /* parse_G_code */

        /* ------------------------------------------------------------------ */
        /*! This function reads the numeric value of a D code and updates the
         *  state.  It also updates the D stats counters
         */
        public static void parse_D_code(gerb_file_t fd, gerb_state_t state, gerbv_image_t image, long line_num_p) {
            int a;
            gerbv_stats_t stats = image.gerbv_stats;
            gerbv_error_list_t error_list = stats.error_list;
            int getintlen = 0;
            a = gerb_fgetint(fd, out getintlen);
            //dprintf("     Found D%02d code at line %ld\n", a, *line_num_p);
        
            switch (a) {
                case 0: /* Invalid code */
                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Found invalid D00 code at line {line_num_p} in file \"{fd.filename}\"");
                    stats.D_error++;
                    break;
                case 1: /* Exposure on */
                    state.aperture_state = gerbv_aperture_state_t.GERBV_APERTURE_STATE_ON;
                    state.changed = 1;
                    stats.D1++;
                    break;
                case 2: /* Exposure off */
                    state.aperture_state = gerbv_aperture_state_t.GERBV_APERTURE_STATE_OFF;
                    state.changed = 1;
                    stats.D2++;
                    break;
                case 3: /* Flash aperture */
                    state.aperture_state = gerbv_aperture_state_t.GERBV_APERTURE_STATE_FLASH;
                    state.changed = 1;
                    stats.D3++;
                    break;
                default: /* Aperture in use */
                    if ((a >= 0) && (a <= APERTURE_MAX)) {
                        state.curr_aperture = a;
                    } else {
                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Found out of bounds aperture D{a} at line {line_num_p} in file \"{fd.filename}\"");
                        stats.D_error++;
                    }
                    state.changed = 0;
                    break;
            }
        
            return;
        } /* parse_D_code */

        /* ------------------------------------------------------------------ */
        public static int parse_M_code(gerb_file_t fd, gerbv_image_t image, long line_num_p) {
            int op_int;
            gerbv_stats_t stats = image.gerbv_stats;
            int getintlen = 0;
            op_int = gerb_fgetint(fd, out getintlen);
        
            switch (op_int) {
                case 0: /* Program stop */ stats.M0++; return 1;
                case 1: /* Optional stop */ stats.M1++; return 2;
                case 2: /* End of program */ stats.M2++; return 3;
                default:
                    gerbv_stats_printf(stats.error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Encountered unknown M{op_int} code at line {line_num_p} in file \"{fd.filename}\"");
                    gerbv_stats_printf(stats.error_list, gerbv_message_type_t.GERBV_MESSAGE_WARNING, -1, $"Ignoring unknown M{op_int} code");
                    stats.M_unknown++;
                    break;
            }
            return 0;
        } /* parse_M_code */


        /* ------------------------------------------------------------------ */
        //static void parse_rs274x(int levelOfRecursion, gerb_file_t fd, gerbv_image_t image, gerb_state_t state, gerbv_net_t curr_net, gerbv_stats_t stats, string directoryPath, long line_num_p) {
        //    char[]               op = new char[2];
        //    char[]              str = new char[3];
        //    int                 tmp;
        //    gerbv_aperture_t    a = null;
        //    gerbv_amacro_t      tmp_amacro;
        //    int                 ano;
        //    double              scale      = 1.0;
        //    gerbv_error_list_t error_list = stats.error_list;
        //
        //    char EOF = char.MaxValue;
        //
        //    if (state.state.unit == gerbv_unit_t.GERBV_UNIT_MM)
        //        scale = 25.4;
        //
        //    op[0] = gerb_fgetc(fd);
        //    op[1] = gerb_fgetc(fd);
        //
        //    if (op[0] == EOF || op[1] == EOF)
        //        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unexpected EOF found in file \"{fd.filename}\"");
        //
        //    string instruction = new string(op);
        //    switch (instruction) {
        //
        //        /*
        //        * Directive parameters
        //        */
        //        case "AS": /* Axis Select */
        //            op[0]        = gerb_fgetc(fd);
        //            op[1]        = gerb_fgetc(fd);
        //            state.state = gerbv_image_return_new_netstate(state.state);
        //
        //            if (op[0] == EOF || op[1] == EOF)
        //                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unexpected EOF found in file \"{fd.filename}\"");
        //
        //            if (((op[0] == 'A') && (op[1] == 'Y')) || ((op[0] == 'B') && (op[1] == 'X'))) {
        //                state.state.axisSelect = gerbv_axis_select_t.GERBV_AXIS_SELECT_SWAPAB;
        //            } else {
        //                state.state.axisSelect = gerbv_axis_select_t.GERBV_AXIS_SELECT_NOSELECT;
        //            }
        //
        //            op[0] = gerb_fgetc(fd);
        //            op[1] = gerb_fgetc(fd);
        //
        //            if (op[0] == EOF || op[1] == EOF)
        //                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unexpected EOF found in file \"{fd.filename}\"");
        //
        //            if (((op[0] == 'A') && (op[1] == 'Y')) || ((op[0] == 'B') && (op[1] == 'X'))) {
        //                state.state.axisSelect = gerbv_axis_select_t.GERBV_AXIS_SELECT_SWAPAB;
        //            } else {
        //                state.state.axisSelect = gerbv_axis_select_t.GERBV_AXIS_SELECT_NOSELECT;
        //            }
        //            break;
        //
        //        case "FS": /* Format Statement */
        //            image.format = new gerbv_format_t();
        //
        //            switch (gerb_fgetc(fd)) {
        //                case 'L': image.format.omit_zeros = gerbv_omit_zeros_t.GERBV_OMIT_ZEROS_LEADING; break;
        //                case 'T': image.format.omit_zeros = gerbv_omit_zeros_t.GERBV_OMIT_ZEROS_TRAILING; break;
        //                case 'D': image.format.omit_zeros = gerbv_omit_zeros_t.GERBV_OMIT_ZEROS_EXPLICIT; break;
        //                default:
        //                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"EagleCad bug detected: Undefined handling of zeros in format code at line {line_num_p} in file \"{fd.filename}\"");
        //                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_WARNING, -1, "Defaulting to omitting leading zeros");
        //                    gerb_ungetc(fd);
        //                    image.format.omit_zeros = gerbv_omit_zeros_t.GERBV_OMIT_ZEROS_LEADING;
        //                    break;
        //            }
        //
        //            switch (gerb_fgetc(fd)) {
        //                case 'A': image.format.coordinate = gerbv_coordinate_t.GERBV_COORDINATE_ABSOLUTE; break;
        //                case 'I': image.format.coordinate = gerbv_coordinate_t.GERBV_COORDINATE_INCREMENTAL; break;
        //                default:
        //                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Invalid coordinate type defined in format code at line {line_num_p} in file \"{fd.filename}\"");
        //                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_WARNING, -1, "Defaulting to absolute coordinates");
        //                    image.format.coordinate = gerbv_coordinate_t.GERBV_COORDINATE_ABSOLUTE;
        //                    break;
        //            }
        //            op[0] = gerb_fgetc(fd);
        //            while ((op[0] != '*') && (op[0] != EOF)) {
        //                switch (op[0]) {
        //                    case 'N':
        //                        op[0]                 = (char)gerb_fgetc(fd);
        //                        image.format.lim_seqno = op[0] - '0';
        //                        break;
        //                    case 'G':
        //                        op[0]                 = (char)gerb_fgetc(fd);
        //                        image.format.lim_gf = op[0] - '0';
        //                        break;
        //                    case 'D':
        //                        op[0]                 = (char)gerb_fgetc(fd);
        //                        image.format.lim_pf = op[0] - '0';
        //                        break;
        //                    case 'M':
        //                        op[0]                 = (char)gerb_fgetc(fd);
        //                        image.format.lim_mf = op[0] - '0';
        //                        break;
        //                    case 'X':
        //                        op[0] = gerb_fgetc(fd);
        //                        if ((op[0] < '0') || (op[0] > '6')) {
        //                            gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Illegal format size '{gerbv_escape_char(op[0])}' at line {line_num_p} in file \"{fd.filename}\"");
        //                        }
        //                        image.format.x_int = op[0] - '0';
        //                        op[0]                = gerb_fgetc(fd);
        //                        if ((op[0] < '0') || (op[0] > '6')) {
        //                            gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Illegal format size '{gerbv_escape_char(op[0])}' at line {line_num_p} in file \"{fd.filename}\"");
        //                        }
        //                        image.format.x_dec = op[0] - '0';
        //                        break;
        //                    case 'Y':
        //                        op[0] = gerb_fgetc(fd);
        //                        if ((op[0] < '0') || (op[0] > '6')) {
        //                            gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Illegal format size '{gerbv_escape_char(op[0])}' at line {line_num_p} in file \"{fd.filename}\"");
        //                        }
        //                        image.format.y_int = op[0] - '0';
        //                        op[0]                = gerb_fgetc(fd);
        //                        if ((op[0] < '0') || (op[0] > '6')) {
        //                            gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Illegal format size '{gerbv_escape_char(op[0])}' at line {line_num_p} in file \"{fd.filename}\"");
        //                        }
        //                        image.format.y_dec = op[0] - '0';
        //                        break;
        //                    default:
        //                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Illegal format statement '{gerbv_escape_char(op[0])}' at line {line_num_p} in file \"{fd.filename}\"");
        //                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_WARNING, -1, "Ignoring invalid format statement");
        //                        break;
        //                }
        //                op[0] = gerb_fgetc(fd);
        //            }
        //            break;
        //        case "MI": /* Mirror Image */
        //            op[0]        = gerb_fgetc(fd);
        //            state.state = gerbv_image_return_new_netstate(state.state);
        //
        //            while ((op[0] != '*') && (op[0] != EOF)) {
        //                int readValue = 0;
        //                int rdlen = 0;
        //                switch (op[0]) {
        //                    case 'A':
        //                        readValue = gerb_fgetint(fd, out rdlen);
        //                        if (readValue == 1) {
        //                            if (state.state.mirrorState == gerbv_mirror_state_t.GERBV_MIRROR_STATE_FLIPB)
        //                                state.state.mirrorState = gerbv_mirror_state_t.GERBV_MIRROR_STATE_FLIPAB;
        //                            else
        //                                state.state.mirrorState = gerbv_mirror_state_t.GERBV_MIRROR_STATE_FLIPA;
        //                        }
        //                        break;
        //                    case 'B':
        //                        readValue = gerb_fgetint(fd, out rdlen);
        //                        if (readValue == 1) {
        //                            if (state.state.mirrorState == gerbv_mirror_state_t.GERBV_MIRROR_STATE_FLIPA)
        //                                state.state.mirrorState = gerbv_mirror_state_t.GERBV_MIRROR_STATE_FLIPAB;
        //                            else
        //                                state.state.mirrorState = gerbv_mirror_state_t.GERBV_MIRROR_STATE_FLIPB;
        //                        }
        //                        break;
        //                    default:
        //                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Wrong character '{gerbv_escape_char(op[0])}' in mirror at line {line_num_p} in file \"{fd.filename}\"");
        //                        break;
        //                }
        //                op[0] = gerb_fgetc(fd);
        //            }
        //            break;
        //        case "MO": /* Mode of Units */
        //            op[0] = gerb_fgetc(fd);
        //            op[1] = gerb_fgetc(fd);
        //
        //            if (op[0] == EOF || op[1] == EOF)
        //                gerbv_stats_printf(
        //                    error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unexpected EOF found in file \"{fd.filename}\"");
        //
        //            switch (new string(op[0], op[1])) {
        //                case "IN":
        //                    state.state       = gerbv_image_return_new_netstate(state.state);
        //                    state.state.unit = gerbv_unit_t.GERBV_UNIT_INCH;
        //                    break;
        //                case "MM":
        //                    state.state       = gerbv_image_return_new_netstate(state.state);
        //                    state.state.unit = gerbv_unit_t.GERBV_UNIT_MM;
        //                    break;
        //                default:
        //                    gerbv_stats_printf(
        //                        error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Illegal unit '{gerbv_escape_char(op[0])}{gerbv_escape_char(op[1])}' at line {line_num_p} in file \"{fd.filename}\"");
        //                    break;
        //            }
        //            break;
        //        case "OF": /* Offset */
        //            op[0] = gerb_fgetc(fd);
        //
        //            while ((op[0] != '*') && (op[0] != EOF)) {
        //                switch (op[0]) {
        //                    case 'A': state.state.offsetA = gerb_fgetdouble(fd) / scale; break;
        //                    case 'B': state.state.offsetB = gerb_fgetdouble(fd) / scale; break;
        //                    default:
        //                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Wrong character '{gerbv_escape_char(op[0])}' in offset at line {line_num_p} in file \"{fd.filename}\"");
        //                        break;
        //                }
        //                op[0] = gerb_fgetc(fd);
        //            }
        //            break;
        //        case "IF": /* Include file */
        //            {
        //                string includeFilename = gerb_fgetstring(fd, '*');
        //
        //                if (!string.IsNullOrEmpty(includeFilename)) {
        //                    string fullPath;
        //                    if (!g_path_is_absolute(includeFilename)) {
        //                        fullPath = g_build_filename(directoryPath, includeFilename, NULL);
        //                    } else {
        //                        fullPath = g_strdup(includeFilename);
        //                    }
        //                    if (levelOfRecursion < 10) {
        //                        gerb_file_t includefd = null;
        //
        //                        includefd = gerb_fopen(fullPath);
        //                        if (includefd) {
        //                            gerber_parse_file_segment(
        //                                levelOfRecursion + 1, image, state, curr_net, stats, includefd, directoryPath
        //                            );
        //                            gerb_fclose(includefd);
        //                        } else {
        //                            gerbv_stats_printf(
        //                                error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Included file \"{fullPath}\" cannot be found at line {line_num_p} in file \"{fd.filename}\"");
        //                        }
        //                        //g_free(fullPath);
        //                    } else {
        //                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, "Parser encountered more than 10 levels of include file recursion which is not allowed by the RS-274X spec");
        //                    }
        //                    //g_free(includeFilename);
        //                }
        //            }
        //            break;
        //        case "IO": /* Image offset */
        //            op[0] = gerb_fgetc(fd);
        //
        //            while ((op[0] != '*') && (op[0] != EOF)) {
        //                switch (op[0]) {
        //                    case 'A': image.info.offsetA = gerb_fgetdouble(fd) / scale; break;
        //                    case 'B': image.info.offsetB = gerb_fgetdouble(fd) / scale; break;
        //                    default:
        //                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Wrong character '{gerbv_escape_char(op[0])}' in image offset at line {line_num_p} in file \"{fd.filename}\"");
        //                        break;
        //                }
        //                op[0] = gerb_fgetc(fd);
        //            }
        //            break;
        //        case "SF": /* Scale Factor */
        //            state.state = gerbv_image_return_new_netstate(state.state);
        //            if (gerb_fgetc(fd) == 'A')
        //                state.state.scaleA = gerb_fgetdouble(fd);
        //            else
        //                gerb_ungetc(fd);
        //            if (gerb_fgetc(fd) == 'B')
        //                state.state.scaleB = gerb_fgetdouble(fd);
        //            else
        //                gerb_ungetc(fd);
        //            break;
        //        case "IC": /* Input Code */
        //            /* Thanks to Stephen Adam for providing this information. As he writes:
        //            *      btw, here's a logic puzzle for you.  If you need to
        //            * read the gerber file to see how it's encoded, then
        //            * how can you read it?
        //            */
        //            op[0] = gerb_fgetc(fd);
        //            op[1] = gerb_fgetc(fd);
        //
        //            if (op[0] == EOF || op[1] == EOF)
        //                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unexpected EOF found in file \"{fd.filename}\"");
        //
        //            switch (new string(op[0], op[1])) {
        //                case "AS": image.info.encoding = gerbv_encoding_t.GERBV_ENCODING_ASCII; break;
        //                case "EB": image.info.encoding = gerbv_encoding_t.GERBV_ENCODING_EBCDIC; break;
        //                case "BC": image.info.encoding = gerbv_encoding_t.GERBV_ENCODING_BCD; break;
        //                case "IS": image.info.encoding = gerbv_encoding_t.GERBV_ENCODING_ISO_ASCII; break;
        //                case "EI": image.info.encoding = gerbv_encoding_t.GERBV_ENCODING_EIA; break;
        //                default:
        //                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unknown input code (IC) '{gerbv_escape_char(op[0])}{gerbv_escape_char(op[1])}' at line {line_num_p} in file \"{fd.filename}\"");
        //                    break;
        //            }
        //            break;
        //
        //        /* Image parameters */
        //        case "IJ": /* Image Justify */
        //            op[0]                          = gerb_fgetc(fd);
        //            image.info.imageJustifyTypeA   = gerbv_image_justify_type_t.GERBV_JUSTIFY_LOWERLEFT;
        //            image.info.imageJustifyTypeB   = gerbv_image_justify_type_t.GERBV_JUSTIFY_LOWERLEFT;
        //            image.info.imageJustifyOffsetA = 0.0;
        //            image.info.imageJustifyOffsetB = 0.0;
        //            while ((op[0] != '*') && (op[0] != EOF)) {
        //                switch (op[0]) {
        //                    case 'A':
        //                        op[0] = gerb_fgetc(fd);
        //                        if (op[0] == 'C') {
        //                            image.info.imageJustifyTypeA = gerbv_image_justify_type_t.GERBV_JUSTIFY_CENTERJUSTIFY;
        //                        } else if (op[0] == 'L') {
        //                            image.info.imageJustifyTypeA = gerbv_image_justify_type_t.GERBV_JUSTIFY_LOWERLEFT;
        //                        } else {
        //                            gerb_ungetc(fd);
        //                            image.info.imageJustifyOffsetA = gerb_fgetdouble(fd) / scale;
        //                        }
        //                        break;
        //                    case 'B':
        //                        op[0] = gerb_fgetc(fd);
        //                        if (op[0] == 'C') {
        //                            image.info.imageJustifyTypeB = gerbv_image_justify_type_t.GERBV_JUSTIFY_CENTERJUSTIFY;
        //                        } else if (op[0] == 'L') {
        //                            image.info.imageJustifyTypeB = gerbv_image_justify_type_t.GERBV_JUSTIFY_LOWERLEFT;
        //                        } else {
        //                            gerb_ungetc(fd);
        //                            image.info.imageJustifyOffsetB = gerb_fgetdouble(fd) / scale;
        //                        }
        //                        break;
        //                    default:
        //                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Wrong character '{gerbv_escape_char(op[0])}' in image justify at line {line_num_p} in file \"{fd.filename}\"");
        //                        break;
        //                }
        //                op[0] = gerb_fgetc(fd);
        //            }
        //            break;
        //        case "IN": /* Image Name */ image.info.name = gerb_fgetstring(fd, '*'); break;
        //        case "IP": /* Image Polarity */
        //
        //            for (ano = 0; ano < 3; ano++) {
        //                op[0] = gerb_fgetc(fd);
        //                if (op[0] == EOF) {
        //                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unexpected EOF while reading image polarity (IP) in file \"{fd.filename}\"");
        //                }
        //                str[ano] = (char)op[0];
        //            }
        //
        //            if (new string(str) == "POS")
        //                image.info.polarity = gerbv_polarity_t.GERBV_POLARITY_POSITIVE;
        //            else if (new string(str) == "NEG")
        //                image.info.polarity = gerbv_polarity_t.GERBV_POLARITY_NEGATIVE;
        //            else {
        //                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unknown polarity '{gerbv_escape_char(str[0])}{gerbv_escape_char(str[1])}{gerbv_escape_char(str[2])}' at line {line_num_p} in file \"{fd.filename}\"");
        //            }
        //            break;
        //        case "IR": /* Image Rotation */
        //            int cntRead = 0;
        //            tmp = gerb_fgetint(fd, out cntRead) % 360;
        //            if (tmp == 0)
        //                image.info.imageRotation = 0.0;
        //            else if (tmp == 90)
        //                image.info.imageRotation = Math.PI / 2.0;
        //            else if (tmp == 180)
        //                image.info.imageRotation = Math.PI;
        //            else if (tmp == 270)
        //                image.info.imageRotation = Math.PI + Math.PI / 2.0;
        //            else {
        //                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Image rotation must be 0, 90, 180 or 270 (is actually {tmp}) at line {line_num_p} in file \"{fd.filename}\"");
        //            }
        //            break;
        //        case "PF": /* Plotter Film */ image.info.plotterFilm = gerb_fgetstring(fd, '*'); break;
        //
        //        /* Aperture parameters */
        //        case "AD": /* Aperture Description */
        //            a = new gerbv_aperture_t();
        //
        //            ano = parse_aperture_definition(fd, a, image, scale, line_num_p);
        //            if (ano == -1) {
        //                /* error with line parse, so just quietly ignore */
        //            } else if ((ano >= 0) && (ano <= APERTURE_MAX)) {
        //                a.unit              = state.state.unit;
        //                image.aperture[ano] = a;
        //                //dprintf("     In %s(), adding new aperture to aperture list ...\n", __func__);
        //                gerbv_stats_add_aperture(stats.aperture_list, -1, ano, a.type, a.parameter);
        //                gerbv_stats_add_to_D_list(stats.D_code_list, ano);
        //                if (ano < APERTURE_MIN) {
        //                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1,
        //                        "Aperture number out of bounds %d at line %ld in file \"%s\"",
        //                        ano, *line_num_p, fd.filename
        //                    );
        //                }
        //            } else {
        //                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1,
        //                    "Aperture number out of bounds %d at line %ld in file \"%s\"",
        //                    ano, *line_num_p, fd.filename
        //                );
        //            }
        //            /* Add aperture info to stats.aperture_list here */
        //
        //            break;
        //        case "AM": /* Aperture Macro */
        //            tmp_amacro    = image.amacro;
        //            image.amacro = parse_aperture_macro(fd);
        //            if (image.amacro != null) {
        //                image.amacro.next = tmp_amacro;
        ////#ifdef AMACRO_DEBUG
        ////                print_program(image.amacro);
        ////#endif
        //            } else {
        //                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, "Failed to parse aperture macro at line %ld in file \"%s\"",
        //                    *line_num_p, fd.filename
        //                );
        //            }
        //            // return, since we want to skip the later back-up loop
        //            return;
        //        /* Layer */
        //        case "LN": /* Layer Name */
        //            state.layer       = gerbv_image_return_new_layer(state.layer);
        //            state.layer.name = gerb_fgetstring(fd, '*');
        //            break;
        //        case "LP": /* Layer Polarity */
        //            state.layer = gerbv_image_return_new_layer(state.layer);
        //            switch (gerb_fgetc(fd)) {
        //                case 'D': /* Dark Polarity (default) */ state.layer.polarity = gerbv_polarity_t.GERBV_POLARITY_DARK; break;
        //                case 'C': /* Clear Polarity */ state.layer.polarity = gerbv_polarity_t.GERBV_POLARITY_CLEAR; break;
        //                default:
        //                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, "Unknown layer polarity '%s' at line %ld in file \"%s\"",
        //                        gerbv_escape_char(op[0]), *line_num_p, fd.filename
        //                    );
        //                    break;
        //            }
        //            break;
        //        case "KO": /* Knock Out */
        //            state.layer = gerbv_image_return_new_layer(state.layer);
        //            gerber_update_any_running_knockout_measurements(image);
        //            /* reset any previous knockout measurements */
        //            knockoutMeasure = false;
        //            op[0]           = gerb_fgetc(fd);
        //            if (op[0] == '*') { /* Disable previous SR parameters */
        //                state.layer.knockout.type = gerbv_knockout_type_t.GERBV_KNOCKOUT_TYPE_NOKNOCKOUT;
        //                break;
        //            } else if (op[0] == 'C') {
        //                state.layer.knockout.polarity = gerbv_polarity_t.GERBV_POLARITY_CLEAR;
        //            } else if (op[0] == 'D') {
        //                state.layer.knockout.polarity = gerbv_polarity_t.GERBV_POLARITY_DARK;
        //            } else {
        //                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, "Knockout must supply a polarity (C, D, or *) at line %ld in file \"%s\"",
        //                    *line_num_p, fd.filename
        //                );
        //            }
        //            state.layer.knockout.lowerLeftX    = 0.0;
        //            state.layer.knockout.lowerLeftY    = 0.0;
        //            state.layer.knockout.width         = 0.0;
        //            state.layer.knockout.height        = 0.0;
        //            state.layer.knockout.border        = 0.0;
        //            state.layer.knockout.firstInstance = true;
        //            op[0]                                = gerb_fgetc(fd);
        //            while ((op[0] != '*') && (op[0] != EOF)) {
        //                switch (op[0]) {
        //                    case 'X':
        //                        state.layer.knockout.type       = gerbv_knockout_type_t.GERBV_KNOCKOUT_TYPE_FIXEDKNOCK;
        //                        state.layer.knockout.lowerLeftX = gerb_fgetdouble(fd) / scale;
        //                        break;
        //                    case 'Y':
        //                        state.layer.knockout.type       = gerbv_knockout_type_t.GERBV_KNOCKOUT_TYPE_FIXEDKNOCK;
        //                        state.layer.knockout.lowerLeftY = gerb_fgetdouble(fd) / scale;
        //                        break;
        //                    case 'I':
        //                        state.layer.knockout.type  = gerbv_knockout_type_t.GERBV_KNOCKOUT_TYPE_FIXEDKNOCK;
        //                        state.layer.knockout.width = gerb_fgetdouble(fd) / scale;
        //                        break;
        //                    case 'J':
        //                        state.layer.knockout.type   = gerbv_knockout_type_t.GERBV_KNOCKOUT_TYPE_FIXEDKNOCK;
        //                        state.layer.knockout.height = gerb_fgetdouble(fd) / scale;
        //                        break;
        //                    case 'K':
        //                        state.layer.knockout.type   = gerbv_knockout_type_t.GERBV_KNOCKOUT_TYPE_BORDER;
        //                        state.layer.knockout.border = gerb_fgetdouble(fd) / scale;
        //                        /* this is a bordered knockout, so we need to start measuring the
        //                        size of a square bordering all future components */
        //                        knockoutMeasure   = true;
        //                        knockoutLimitXmin = HUGE_VAL;
        //                        knockoutLimitYmin = HUGE_VAL;
        //                        knockoutLimitXmax = -HUGE_VAL;
        //                        knockoutLimitYmax = -HUGE_VAL;
        //                        knockoutLayer     = state.layer;
        //                        break;
        //                    default:
        //                        gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unknown variable in knockout at line {line_num_p} in file \"{fd.filename}\"");
        //                        break;
        //                }
        //                op[0] = gerb_fgetc(fd);
        //            }
        //            break;
        //        case "SR": /* Step and Repeat */
        //            /* start by generating a new layer (duplicating previous layer settings */
        //            state.layer = gerbv_image_return_new_layer(state.layer);
        //            op[0]        = gerb_fgetc(fd);
        //            if (op[0] == '*') { /* Disable previous SR parameters */
        //                state.layer.stepAndRepeat.X      = 1;
        //                state.layer.stepAndRepeat.Y      = 1;
        //                state.layer.stepAndRepeat.dist_X = 0.0;
        //                state.layer.stepAndRepeat.dist_Y = 0.0;
        //                break;
        //            }
        //            while ((op[0] != '*') && (op[0] != EOF)) {
        //                switch (op[0]) {
        //                    case 'X': state.layer.stepAndRepeat.X = gerb_fgetint(fd, NULL); break;
        //                    case 'Y': state.layer.stepAndRepeat.Y = gerb_fgetint(fd, NULL); break;
        //                    case 'I': state.layer.stepAndRepeat.dist_X = gerb_fgetdouble(fd) / scale; break;
        //                    case 'J': state.layer.stepAndRepeat.dist_Y = gerb_fgetdouble(fd) / scale; break;
        //                    default:
        //                        gerbv_stats_printf(
        //                            error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1,
        //                            "Step-and-repeat parameter error at line %ld in file \"%s\"",
        //                            *line_num_p, fd.filename
        //                        );
        //                        break;
        //                }
        //
        //                /*
        //                * Repeating 0 times in any direction would disable the whole plot, and
        //                * is probably not intended. At least one other tool (viewmate) seems
        //                * to interpret 0-time repeating as repeating just once too.
        //                */
        //                if (state.layer.stepAndRepeat.X == 0)
        //                    state.layer.stepAndRepeat.X = 1;
        //                if (state.layer.stepAndRepeat.Y == 0)
        //                    state.layer.stepAndRepeat.Y = 1;
        //
        //                op[0] = gerb_fgetc(fd);
        //            }
        //            break;
        //        /* is this an actual RS274X command??  It isn't explainined in the spec... */
        //        case "RO":
        //            state.layer = gerbv_image_return_new_layer(state.layer);
        //
        //            state.layer.rotation = DEG2RAD(gerb_fgetdouble(fd));
        //            op[0]                  = gerb_fgetc(fd);
        //            if (op[0] != '*') {
        //                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Error in layer rotation command at line {line_num_p} in file \"{fd.filename}\"");
        //            }
        //            break;
        //        default:
        //            gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Unknown RS-274X extension found %%{gerbv_escape_char(op[0])}{gerbv_escape_char(op[1])}%% at line {line_num_p} in file \"{fd.filename}\"");
        //            break;
        //    }
        //
        //    // make sure we read until the trailing * character
        //    // first, backspace once in case we already read the trailing *
        //    gerb_ungetc(fd);
        //    do {
        //        tmp = gerb_fgetc(fd);
        //    } while (tmp != EOF && tmp != '*');
        //
        //    return;
        //} /* parse_rs274x */

        /*
         * Stack declarations and operations to be used by the simple engine that
         * executes the parsed aperture macros.
         */
        public class macro_stack_t {
            public double[] stack;
            public int sp;
            public int capacity;
        };

        public static macro_stack_t new_stack(int stack_size) {
            macro_stack_t s;

            s = new macro_stack_t();
            s.stack = new double[stack_size];
            s.sp = 0;
            s.capacity = stack_size;
            return s;
        } /* new_stack */

        public static void free_stack(macro_stack_t s) {
            //if (s && s.stack)
            //    free(s.stack);
            //
            //if (s)
            //    free(s);
            //
            //return;
        } /* free_stack */

        public static void push(macro_stack_t s, double val) {
            if (s.sp >= s.capacity) {
                GERB_FATAL_ERROR("push will overflow stack capacity");
                return;
            }
            s.stack[s.sp++] = val;
            return;
        } /* push */

        public static int pop(macro_stack_t s, ref double value) {
            /* Check if we try to pop an empty stack */
            if (s.sp == 0) {
                return -1;
            }

            value = s.stack[--s.sp];
            return 0;
        } /* pop */

        /* ------------------------------------------------------------------ */
        public static int simplify_aperture_macro(gerbv_aperture_t aperture, double scale) {
            const int extra_stack_size = 10;
            macro_stack_t s;
            gerbv_instruction_t ip;
            int handled = 1, nuf_parameters = 0, i, j;
            bool clearOperatorUsed = false;
            double[] lp; /* Local copy of parameters */
            double[] tmp = [0.0, 0.0];
            gerbv_aperture_type_t type = gerbv_aperture_type_t.GERBV_APTYPE_NONE;
            gerbv_simplified_amacro_t sam;

            if (aperture == null)
                GERB_FATAL_ERROR("aperture NULL in simplify aperture macro");

            if (aperture.amacro == null)
                GERB_FATAL_ERROR("aperture.amacro NULL in simplify aperture macro");

            /* Allocate stack for VM */
            s = new_stack((int)(aperture.amacro.nuf_push + extra_stack_size));
            if (s == null)
                GERB_FATAL_ERROR("malloc stack failed in %s()");

            /* Make a copy of the parameter list that we can rewrite if necessary */
            lp = new double[APERTURE_PARAMETERS_MAX];
            Array.Copy(aperture.parameter, 0, lp, 0, APERTURE_PARAMETERS_MAX);

            for (ip = aperture.amacro.program; ip != null; ip = ip.next) {
                switch (ip.opcode) {
                    case gerbv_opcodes_t.GERBV_OPCODE_NOP: break;
                    case gerbv_opcodes_t.GERBV_OPCODE_PUSH: push(s, ip.data.fval); break;
                    case gerbv_opcodes_t.GERBV_OPCODE_PPUSH: {
                            long idx = ip.data.ival - 1;
                            if ((idx < 0) || (idx >= APERTURE_PARAMETERS_MAX))
                                GERB_FATAL_ERROR("Tried to access oob aperture");
                            push(s, lp[idx]);
                            break;
                        }
                    case gerbv_opcodes_t.GERBV_OPCODE_PPOP: {
                            if (pop(s, ref tmp[0]) < 0)
                                GERB_FATAL_ERROR("Tried to pop an empty stack");
                            long idx = ip.data.ival - 1;
                            if ((idx < 0) || (idx >= APERTURE_PARAMETERS_MAX))
                                GERB_FATAL_ERROR("Tried to access oob aperture");
                            lp[idx] = tmp[0];
                            break;
                        }
                    case gerbv_opcodes_t.GERBV_OPCODE_ADD:
                        if (pop(s, ref tmp[0]) < 0)
                            GERB_FATAL_ERROR("Tried to pop an empty stack");
                        if (pop(s, ref tmp[1]) < 0)
                            GERB_FATAL_ERROR("Tried to pop an empty stack");
                        push(s, tmp[1] + tmp[0]);
                        break;
                    case gerbv_opcodes_t.GERBV_OPCODE_SUB:
                        if (pop(s, ref tmp[0]) < 0)
                            GERB_FATAL_ERROR("Tried to pop an empty stack");
                        if (pop(s, ref tmp[1]) < 0)
                            GERB_FATAL_ERROR("Tried to pop an empty stack");
                        push(s, tmp[1] - tmp[0]);
                        break;
                    case gerbv_opcodes_t.GERBV_OPCODE_MUL:
                        if (pop(s, ref tmp[0]) < 0)
                            GERB_FATAL_ERROR("Tried to pop an empty stack");
                        if (pop(s, ref tmp[1]) < 0)
                            GERB_FATAL_ERROR("Tried to pop an empty stack");
                        push(s, tmp[1] * tmp[0]);
                        break;
                    case gerbv_opcodes_t.GERBV_OPCODE_DIV:
                        if (pop(s, ref tmp[0]) < 0)
                            GERB_FATAL_ERROR("Tried to pop an empty stack");
                        if (pop(s, ref tmp[1]) < 0)
                            GERB_FATAL_ERROR("Tried to pop an empty stack");
                        push(s, tmp[1] / tmp[0]);
                        break;
                    case gerbv_opcodes_t.GERBV_OPCODE_PRIM:
                        /*
                         * This handles the exposure thing in the aperture macro
                         * The exposure is always the first element on stack independent
                         * of aperture macro.
                         */
                        switch (ip.data.ival) {
                            case 1:
                                //dprintf("  Aperture macro circle [1] (");
                                type = gerbv_aperture_type_t.GERBV_APTYPE_MACRO_CIRCLE;
                                nuf_parameters = 4;
                                break;
                            case 3: break;
                            case 4:
                                //dprintf("  Aperture macro outline [4] (");
                                type = gerbv_aperture_type_t.GERBV_APTYPE_MACRO_OUTLINE;
                                /*
                                 * Number of parameters are:
                                 * - number of points defined in entry 1 of the stack +
                                 *   start point. Times two since it is both X and Y.
                                 * - Then three more; exposure,  nuf points and rotation.
                                 *
                                 * @warning Calculation must be guarded against signed integer
                                 *     overflow
                                 *
                                 * @see CVE-2021-40394
                                 */
                                int sstack = (int)s.stack[1];
                                if ((sstack < 0) || (sstack >= int.MaxValue / 4)) {
                                    GERB_COMPILE_ERROR($"Possible signed integer overflow in calculating number of parameters to aperture macro, will clamp to ({APERTURE_PARAMETERS_MAX})");
                                    nuf_parameters = APERTURE_PARAMETERS_MAX;
                                } else {
                                    nuf_parameters = (sstack + 1) * 2 + 3;
                                }
                                break;
                            case 5:
                                //dprintf("  Aperture macro polygon [5] (");
                                type = gerbv_aperture_type_t.GERBV_APTYPE_MACRO_POLYGON;
                                nuf_parameters = 6;
                                break;
                            case 6:
                                //dprintf("  Aperture macro moire [6] (");
                                type = gerbv_aperture_type_t.GERBV_APTYPE_MACRO_MOIRE;
                                nuf_parameters = 9;
                                break;
                            case 7:
                                //dprintf("  Aperture macro thermal [7] (");
                                type = gerbv_aperture_type_t.GERBV_APTYPE_MACRO_THERMAL;
                                nuf_parameters = 6;
                                break;
                            case 2:
                            case 20:
                                //dprintf("  Aperture macro line 20/2 (");
                                type = gerbv_aperture_type_t.GERBV_APTYPE_MACRO_LINE20;
                                nuf_parameters = 7;
                                break;
                            case 21:
                                //dprintf("  Aperture macro line 21 (");
                                type = gerbv_aperture_type_t.GERBV_APTYPE_MACRO_LINE21;
                                nuf_parameters = 6;
                                break;
                            case 22:
                                //dprintf("  Aperture macro line 22 (");
                                type = gerbv_aperture_type_t.GERBV_APTYPE_MACRO_LINE22;
                                nuf_parameters = 6;
                                break;
                            default: handled = 0;
                                break;
                        }

                        if (type != gerbv_aperture_type_t.GERBV_APTYPE_NONE) {
                            if (nuf_parameters > APERTURE_PARAMETERS_MAX) {
                                GERB_COMPILE_ERROR($"Number of parameters to aperture macro ({nuf_parameters}) are more than gerbv is able to store ({APERTURE_PARAMETERS_MAX})");
                                nuf_parameters = APERTURE_PARAMETERS_MAX;
                            }

                            /*
                             * Create struct for simplified aperture macro and
                             * start filling in the blanks.
                             */
                            sam = new gerbv_simplified_amacro_t();
                            sam.type = type;
                            sam.next = null;
                            //memset(sam.parameter, 0, sizeof(double) * APERTURE_PARAMETERS_MAX);

                            /* CVE-2021-40400
                             */
                            if (nuf_parameters > s.capacity) {
                                GERB_COMPILE_ERROR($"Number of parameters to aperture macro ({nuf_parameters}) capped to stack capacity ({s.capacity})");
                                nuf_parameters = s.capacity;
                            }
                            Array.Copy(s.stack, 0, sam.parameter, 0, nuf_parameters);

                            /* convert any mm values to inches */
                            switch (type) {
                                case gerbv_aperture_type_t.GERBV_APTYPE_MACRO_CIRCLE:
                                    if (Math.Abs(sam.parameter[0]) < 0.001)
                                        clearOperatorUsed = true;
                                    sam.parameter[1] /= scale;
                                    sam.parameter[2] /= scale;
                                    sam.parameter[3] /= scale;
                                    break;
                                case gerbv_aperture_type_t.GERBV_APTYPE_MACRO_OUTLINE:
                                    if (Math.Abs(sam.parameter[0]) < 0.001)
                                        clearOperatorUsed = true;
                                    for (j = 2; j < nuf_parameters - 1; j++) {
                                        sam.parameter[j] /= scale;
                                    }
                                    break;
                                case gerbv_aperture_type_t.GERBV_APTYPE_MACRO_POLYGON:
                                    if (Math.Abs(sam.parameter[0]) < 0.001)
                                        clearOperatorUsed = true;
                                    sam.parameter[2] /= scale;
                                    sam.parameter[3] /= scale;
                                    sam.parameter[4] /= scale;
                                    break;
                                case gerbv_aperture_type_t.GERBV_APTYPE_MACRO_MOIRE:
                                    sam.parameter[0] /= scale;
                                    sam.parameter[1] /= scale;
                                    sam.parameter[2] /= scale;
                                    sam.parameter[3] /= scale;
                                    sam.parameter[4] /= scale;
                                    sam.parameter[6] /= scale;
                                    sam.parameter[7] /= scale;
                                    break;
                                case gerbv_aperture_type_t.GERBV_APTYPE_MACRO_THERMAL:
                                    sam.parameter[0] /= scale;
                                    sam.parameter[1] /= scale;
                                    sam.parameter[2] /= scale;
                                    sam.parameter[3] /= scale;
                                    sam.parameter[4] /= scale;
                                    break;
                                case gerbv_aperture_type_t.GERBV_APTYPE_MACRO_LINE20:
                                    if (Math.Abs(sam.parameter[0]) < 0.001)
                                        clearOperatorUsed = true;
                                    sam.parameter[1] /= scale;
                                    sam.parameter[2] /= scale;
                                    sam.parameter[3] /= scale;
                                    sam.parameter[4] /= scale;
                                    sam.parameter[5] /= scale;
                                    break;
                                case gerbv_aperture_type_t.GERBV_APTYPE_MACRO_LINE21:
                                case gerbv_aperture_type_t.GERBV_APTYPE_MACRO_LINE22:
                                    if (Math.Abs(sam.parameter[0]) < 0.001)
                                        clearOperatorUsed = true;
                                    sam.parameter[1] /= scale;
                                    sam.parameter[2] /= scale;
                                    sam.parameter[3] /= scale;
                                    sam.parameter[4] /= scale;
                                    break;
                                default: break;
                            }
                            /*
                             * Add this simplified aperture macro to the end of the list
                             * of simplified aperture macros. If first entry, put it
                             * in the top.
                             */
                            if (aperture.simplified == null) {
                                aperture.simplified = sam;
                            } else {
                                gerbv_simplified_amacro_t tmp_sam;
                                tmp_sam = aperture.simplified;
                                while (tmp_sam.next != null) {
                                    tmp_sam = tmp_sam.next;
                                }
                                tmp_sam.next = sam;
                            }

#if DEBUG
                            for (i = 0; i < nuf_parameters; i++) {
                                //dprintf("%f, ", s.stack[i]);
                            }
#endif 
                            //dprintf(")\n");
                        }

                        /*
                         * Here we reset the stack pointer. It's not general correct
                         * correct to do this, but since I know how the compiler works
                         * I can do this. The correct way to do this should be to
                         * subtract number of used elements in each primitive operation.
                         */
                        s.sp = 0;
                        break;
                    default: break;
                }
            }
            free_stack(s);
            //g_free(lp);

            /* store a flag to let the renderer know if it should expect any "clear"
               primatives */
            aperture.parameter[0] = (double)(clearOperatorUsed ? 1 : 0);
            return handled;
        } /* simplify_aperture_macro */


        /* ------------------------------------------------------------------ */
        public static int parse_aperture_definition(gerb_file_t fd, gerbv_aperture_t aperture, gerbv_image_t image, double scale, long line_num_p) {
            int ano, i;
            string ad;
            string token;
            gerbv_amacro_t curr_amacro = null;
            gerbv_amacro_t amacro = image.amacro;
            gerbv_error_list_t error_list = image.gerbv_stats.error_list;
            double tempHolder;
        
            if (gerb_fgetc(fd) != 'D') {
                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Found AD code with no following 'D' at line {line_num_p} in file \"{fd.filename}\"");
                return -1;
            }
        
            /*
             * Get aperture no
             */
            int readc = 0;
            ano = gerb_fgetint(fd, out readc);
        
            /*
             * Read in the whole aperture defintion and tokenize it
             */
            ad = gerb_fgetstring(fd, '*');
        
            if (string.IsNullOrEmpty(ad)) {
                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Invalid aperture definition at line {line_num_p} in file \"{fd.filename}\", cannot find '*'");
                return -1;
            }
        
            token = libc.strtok(ad, ",");
        
            if (string.IsNullOrEmpty(token)) {
                gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Invalid aperture definition at line {line_num_p} in file \"{fd.filename}\"");
                return -1;
            }
            if (token.Length == 1) {
                switch (token[0]) {
                    case 'C': aperture.type = gerbv_aperture_type_t.GERBV_APTYPE_CIRCLE; break;
                    case 'R': aperture.type = gerbv_aperture_type_t.GERBV_APTYPE_RECTANGLE; break;
                    case 'O': aperture.type = gerbv_aperture_type_t.GERBV_APTYPE_OVAL; break;
                    case 'P': aperture.type = gerbv_aperture_type_t.GERBV_APTYPE_POLYGON; break;
                }
                /* Here a should a T be defined, but I don't know what it represents */
            } else {
                aperture.type = gerbv_aperture_type_t.GERBV_APTYPE_MACRO;
                /*
                 * In aperture definition, point to the aperture macro
                 * used in the defintion
                 */
                curr_amacro = amacro;
                while (curr_amacro != null) {
                    if ((curr_amacro.name.Length == token.Length) && (curr_amacro.name == token)) {
                        aperture.amacro = curr_amacro;
                        break;
                    }
                    curr_amacro = curr_amacro.next;
                }
            }
        
            /*
             * Parse all parameters
             */
            for (token = libc.strtok(null, "X"), i = 0; !string.IsNullOrEmpty(token); token = libc.strtok(null, "X"), i++) {
                if (i == APERTURE_PARAMETERS_MAX) {
                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_ERROR, -1, $"Maximum number of allowed parameters exceeded in aperture {ano} at line {line_num_p} in file \"{fd.filename}\"");
                    break;
                }
        
                bool error = double.TryParse(token, out tempHolder);
                /* convert any MM values to inches */
                /* don't scale polygon angles or side numbers, or macro parmaeters */
                if (!(((aperture.type == gerbv_aperture_type_t.GERBV_APTYPE_POLYGON) && ((i == 1) || (i == 2)))
                      || (aperture.type == gerbv_aperture_type_t.GERBV_APTYPE_MACRO))) {
                    tempHolder /= scale;
                }
        
                aperture.parameter[i] = tempHolder;
                if (error) {
                    gerbv_stats_printf(error_list, gerbv_message_type_t.GERBV_MESSAGE_WARNING, -1, $"Failed to read all parameters exceeded in aperture {ano} at line {line_num_p} in file \"{fd.filename}\"");
                    aperture.parameter[i] = 0.0;
                }
            }
        
            aperture.nuf_parameters = i;
        
            gerb_ungetc(fd);
        
            if (aperture.type == gerbv_aperture_type_t.GERBV_APTYPE_MACRO) {
                //dprintf("Simplifying aperture %d using aperture macro \"%s\"\n", ano, aperture.amacro.name);
                simplify_aperture_macro(aperture, scale);
                //dprintf("Done simplifying\n");
            }
        
            //g_free(ad);
        
            return ano;
        } /* parse_aperture_definition */
    }
}