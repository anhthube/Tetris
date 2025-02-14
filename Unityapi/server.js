const express = require('express');
const mysql = require('mysql');
const bodyParser = require('body-parser');
const cors = require('cors');

// Tạo ứng dụng Express
const app = express();
app.use(cors());
app.use(bodyParser.urlencoded({ extended: true })); // xu ly du lieu duoi dang  application/x-www-form-urlencoded
app.use(bodyParser.json());// xu ly du lieu dang json 

// Kết nối tới MySQL
const db = mysql.createConnection({
    host: '127.0.0.1',
    user: 'root',
    password: '', 
    database: 'unity_game',
});

db.connect((err) => {
    if (err) {
        console.error('Kết nối database thất bại:', err);
        return;
    }
    console.log('Kết nối database thành công!');
});

// Đăng ký người dùng
app.post('/signup', (req, res) => {
    const { username, password } = req.body;
    console.log(username, password);
    if (!username || !password) {
        return res.status(400).json({ error: 'Vui lòng điền đầy đủ thông tin!' });
    }

    const query = 'INSERT INTO users (username, password) VALUES (?, ?)';
    db.query(query, [username, password], (err, result) => {
        if (err) {
            if (err.code === 'ER_DUP_ENTRY') {
                return res.status(409).json({ error: 'Tên người dùng đã tồn tại!' });
            }
            return res.status(500).json({ error: 'Lỗi server!' });
        }
        res.status(201).json({ message: 'Đăng ký thành công!' });
    });
});

// Đăng nhập người dùng
app.post('/login', (req, res) => {
    const { username, password } = req.body;
    console.log(username, password);
    if (!username || !password) {
        return res.status(400).json({ error: 'Vui lòng điền đầy đủ thông tin!' });
    }

    const query = 'SELECT * FROM users WHERE username = ? AND password = ?';
    db.query(query, [username, password], (err, results) => {
        if (err) {
            return res.status(500).json({ error: 'Lỗi server!' });
        }

        if (results.length > 0) {
            res.status(200).json({ message: 'Đăng nhập thành công!' });
        } else {
            res.status(401).json({ error: 'Tên đăng nhập hoặc mật khẩu không đúng!' });
        }
    });
});


// Bắt đầu server
const PORT = 3000;
app.listen(PORT, () => {
    console.log(`API server đang chạy tại http://localhost:${PORT}`);
});
