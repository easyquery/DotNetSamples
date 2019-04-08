const path = require("path");

module.exports = {
    entry: ["./ts/styles.js", "./ts/easyreport.ts"],
    stats: {warnings:false},
	devtool: "source-map",
    output: {
        library: 'easyreport',
        path: path.resolve(__dirname, 'wwwroot/js'),
        filename: "easyreport.js"
    },
    resolve: {
        extensions: [".js", ".ts"]
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: "ts-loader"
            },
			{
				test: /\.css$/,
				loader: 'style-loader'
			},
			{
				test: /\.css$/,
				loader: 'css-loader',
				options: {
				  url: false
				}
			}
        ]
    },
	watchOptions: {
		aggregateTimeout: 2000
	}
};