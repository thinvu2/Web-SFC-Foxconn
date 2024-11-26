module.exports = {
    productionSourceMap: false,
    css: {
        extract: false
    },
    //publicPath: '',
    publicPath: process.env.NODE_ENV === 'production'
        ? ''
        : '/',
    filenameHashing: true,
    chainWebpack: config => {
        config
            .plugin('html')
            .tap(args => {
                args[0].title = "Smart Shopfloor System";
                return args;
            })

        config.module
            .rule('images')
            .use('url-loader')
            .loader('url-loader')
            .tap(options => Object.assign(options, { limit: 10240 }))
    }
    // chainWebpack: config => {
    //     config
    //         .plugin('html')
    //         .tap(args => {
    //             args[0].title = "Smart Shopfloor System";
    //             return args;
    //         })
    // }
}