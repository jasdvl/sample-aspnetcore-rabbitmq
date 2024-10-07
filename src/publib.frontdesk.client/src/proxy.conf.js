const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7287';

const PROXY_CONFIG = [
    {
        context: ["/api"], // Leitet alle API-Aufrufe mit /api-Präfix weiter
        target,
        changeOrigin: true,
        secure: false
    }
]

console.log("Proxy port: " + env.ASPNETCORE_HTTPS_PORT);


module.exports = PROXY_CONFIG;
