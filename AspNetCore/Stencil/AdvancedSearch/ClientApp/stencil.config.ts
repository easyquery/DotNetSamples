import { Config } from '@stencil/core';

// https://stenciljs.com/docs/config

export const config: Config = {
  globalStyle: 'src/global/app.css',
  globalScript: 'src/global/app.ts',
  taskQueue: 'async',
  outputTargets: [
    {
      type: 'www',
      baseUrl: 'http://localhost:3000/'
    },
  ],
  devServer: {
    reloadStrategy: 'pageReload',
    port: 4444,
    openBrowser: false
  }
};