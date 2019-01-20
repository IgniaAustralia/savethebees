export const environment = {
    production: false    
};

export const settings = {
    baseApiUrl: 'https://insightsavethebeesselfservewebapi.azurewebsites.net',
    endPoints: {
        authToken: '/connect/token',
        getUsers: '/api/users'
    },
    authentication: {
        clientId: '32369f53-f6ba-448c-9160-78574060d7a4',
        clientSecret: '6bd213c9-970a-4010-8464-c64e9f488097'
    }
}