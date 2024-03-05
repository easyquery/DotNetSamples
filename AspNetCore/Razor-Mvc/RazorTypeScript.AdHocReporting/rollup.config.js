import {nodeResolve} from '@rollup/plugin-node-resolve'
import terser from '@rollup/plugin-terser'
import progress from 'rollup-plugin-progress'
import typescript from '@rollup/plugin-typescript'
import * as path from "path";
import { fileURLToPath } from 'url';
import noEmit from 'rollup-plugin-no-emit'
import postcss from 'rollup-plugin-postcss'
import autoprefixer from "autoprefixer"
import pkg from './package.json' assert { type: 'json' };
import cleanup from 'rollup-plugin-cleanup'

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const production = !(process.env.ROLLUP_WATCH),
    sourcemap = !production,
    cache = false

const banner = `
/*!
 * Razor AdHoc Reporting Demo v${pkg.version}
 * Copyright ${new Date().getFullYear()} Korzh.com
 * Licensed under MIT
 */
`

const onwarn = warn => {
    if (/Generated an empty chunk/.test(warn) || warn.code === 'FILE_NAME_CONFLICT') return;
    console.error( warn )
}


export default [
    {
        input: './ts/adhoc-reporting.ts',
        cache,
        watch: {
            clearScreen: false
        },
        plugins: [
            progress({ clearLine: true, }),
            cleanup(),
            typescript({
                sourceMap: false,
                declaration: false,
            }),
            nodeResolve({ browser: true, }),
        ],
        output: [
            {
                file: './wwwroot/js/adhoc-reporting.js',
                format: 'iife',
                sourcemap: false,
                banner,
                name: "easyreport"
            },
        ]
    },
    {
        input: './ts/adhoc-reporting.ts',
        cache,
        watch: {
            clearScreen: false
        },
        plugins: [
            progress({ clearLine: true, }),
            cleanup(),
            typescript({
                sourceMap: true,
                declaration: false,
            }),
            nodeResolve({ browser: true, }),
        ],
        output: [
            {
                file: './wwwroot/js/adhoc-reporting.min.js',
                format: 'iife',
                sourcemap: true,
                banner,
                name: "easyreport",
                plugins: [
                    terser({
                        keep_classnames: true,
                        keep_fnames: true,
                    }),
                ],
            },
        ]
    },
    {
        input: './ts/styles.js',
        plugins: [
            progress({
                clearLine: true,
            }),
            nodeResolve(),
            postcss({
                extract: true,
                minimize: false,
                use: ['less'],
                sourceMap: false,
                plugins: [
                    autoprefixer(),
                ]
            }),
            noEmit({
                match(fileName, output) {
                    return 'styles.js' === fileName
                }
            }),
        ],
        output: {
            file: './wwwroot/css/adhoc-reporting.css',
            banner,
        },
        onwarn,
    },
    {
        input: './ts/styles.js',
        plugins: [
            progress({
                clearLine: true,
            }),
            nodeResolve(),
            postcss({
                extract: true,
                minimize: true,
                use: ['less'],
                sourceMap: true,
                plugins: [
                    autoprefixer(),
                ]
            }),
            noEmit({
                match(fileName, output) {
                    return 'styles.js' === fileName
                }
            }),
        ],
        output: {
            file: './wwwroot/css/adhoc-reporting.min.css',
            banner,
        },
        onwarn,
    },
]