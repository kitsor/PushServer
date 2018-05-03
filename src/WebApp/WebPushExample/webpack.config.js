const path = require('path');
const webpack = require('webpack');
const merge = require('webpack-merge');

module.exports = (env) => {
	const isDevBuild = !(env && env.prod);

	const sharedConfig = {
		mode: isDevBuild ? 'development' : 'production',
		resolve: { extensions: ['.ts', '.js'] },
		module: {
			rules: [
				{ test: /\.ts$/, use: 'ts-loader' }
			]
		}
	};

	const outputDir = './wwwroot/js/dist';

	const wpswConfig = merge(sharedConfig, {
		entry: { 'wp-sw': './Scripts/wp-sw.ts' },
		output: { path: path.join(__dirname, './wwwroot') }
	});

	const wpsConfig = merge(sharedConfig, {
		entry: { 'wps': './Scripts/wps.ts' },
		output: { path: path.join(__dirname, outputDir) }
	});

	const keyGeneratorConfig = merge(sharedConfig, {
		entry: { 'keyGenerator': './Scripts/keyGenerator.ts' },
		output: { path: path.join(__dirname, outputDir) }
	});


	return [wpsConfig, wpswConfig, keyGeneratorConfig];
};