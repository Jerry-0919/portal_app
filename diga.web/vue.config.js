module.exports = {
    outputDir: 'Assets/js/vue',
    filenameHashing: false,
    configureWebpack: {
    optimization: {
          splitChunks: false
      },
      resolve: {
          alias: {
              'vue$': 'vue/dist/vue.esm.js'
          }
      }
  },
}