const jsonServer = require('json-server');
const server = jsonServer.create();
const router = jsonServer.router('./fake.json');
const provider = require('./fake.json');
const middlewares = jsonServer.defaults();
const port = process.env.PORT || 3000;

server.use(middlewares);

server.get('/profiles/:id', (req, res) => {
    if (req.method === 'GET') {
        const id = req.params.id;

        if (id == 404) {
            res.status(404).jsonp({ error: 'Not found' });
            return;
        }

        if (id == 400) {
            res.status(400).jsonp({ error: 'Bad Request' });
        }
        
        if (id == 408) {
            res.status(408).jsonp({ error: 'Request Timeout' });
        }

        if (id == 500) {
            res.status(500).jsonp({ error: 'Internal Server Error' });
            return;
        }

        const profile = provider.profiles.find(p => p.id == id);

        if (!profile) {
            res.status(404).jsonp({ error: 'Not found' });
            return;
        } else {
            res.status(200).jsonp(profile);
        }
    }
});

server.use(router);

server.listen(port, () => {
    console.log('JSON Server is running on port ' + port);
});