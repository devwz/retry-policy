const express = require('express');
const server = express();
const port = process.env.PORT || 3000;

server.use(express.json());

server.get('/profiles', (req, res) => {

    const id = req.query.id

    if (id == 404) {
        console.log('404 - GET /profiles');
        res.status(404).jsonp({ error: 'Not found' });
        return;
    }
    else if (id == 400) {
        console.log('400 - GET /profiles');
        res.status(400).jsonp({ error: 'Bad Request' });
    }
    
    else if (id == 408) {
        console.log('408 - GET /profiles');
        res.status(408).jsonp({ error: 'Request Timeout' });
    }

    else if (id == 500) {
        console.log('500 - GET /profiles');
        res.status(500).jsonp({ error: 'Internal Server Error' });
        return;
    }
    else {
        console.log('200 - GET /profiles');

        const profile = {
            id: 1,
            title: 'John Doe',
        }

        res.status(200).jsonp(profile);
    }
});

server.listen(port, () => {
    console.log('Server running on port ' + port);
});