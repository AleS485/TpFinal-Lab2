CREATE DATABASE IF NOT EXISTS gamestore;
USE gamestore;

create table if not exists desarrolladora (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    web VARCHAR(150) NULL
);

create table if not exists usuario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL,
    avatar_url VARCHAR(255) NULL,
    estado TINYINT(1) DEFAULT 1
);

create table if not exists cliente (
    id INT AUTO_INCREMENT PRIMARY KEY,
    dni VARCHAR(20) NOT NULL UNIQUE,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    telefono VARCHAR(50) NULL,
    email VARCHAR(150) NOT NULL
);

create table if not exists juego (
    id int auto_increment primary key,
    titulo varchar(150) not null,
    genero varchar(50) not null,
    descripcion text default null,
    precio decimal(10,2) not null default 0,
    stock int not null default 0,
    estado boolean default true,
    desarrolladora_id int not null,
    constraint fk_juego_desarrolladora foreign key (desarrolladora_id)
        references desarrolladora(id)
        on update cascade
);

create table if not exists imagenjuego (
    id int auto_increment primary key,
    juego_id int not null,
    constraint fk_imagen_juegos foreign key (juego_id)
        references juego(id)
        on delete cascade,
    original_name varchar(250) not null,
    url varchar(250) not null,
    is_portada boolean default false
);

create table if not exists venta (
    id int auto_increment primary key,
    fecha_hora datetime default current_timestamp,
    cantidad int not null default 1,
    precio_total decimal(10,2) not null default 0,
    juego_id int not null,
    cliente_id int not null,
    usuario_id int not null,
    constraint fk_venta_juego foreign key (juego_id)
        references juego(id),
    constraint fk_venta_cliente foreign key (cliente_id)
        references cliente(id),
    constraint fk_venta_usuario foreign key (usuario_id)
        references usuario(id)
);

create table if not exists resena (
    id int auto_increment primary key,
    calificacion int not null,
    comentario text default null,
    fecha datetime default current_timestamp,
    estado boolean default true,
    juego_id int not null,
    cliente_id int not null,
    constraint fk_resena_juego foreign key (juego_id)
        references juego(id),
    constraint fk_resena_cliente foreign key (cliente_id)
        references cliente(id)
);

create table if not exists noticias (
    idNoticia int auto_increment primary key,
    titulo varchar(150) not null,
    contenido text not null,
    fecha datetime default current_timestamp,
    usuarioId int not null,
    constraint fk_noticias_usuario foreign key (usuarioId)
        references usuario(id)
        on update cascade
        on delete cascade
);


-- Datos para rellenar la BD --

INSERT INTO desarrolladora (nombre, web) VALUES 
('Ryu Ga Gotoku Studio', 'https://ryu-ga-gotoku.com'),
('CD Projekt Red', 'https://www.cdprojektred.com'),
('Capcom', 'https://www.capcom.com'),
('FromSoftware', 'https://www.fromsoftware.jp'),
('Square Enix', 'https://www.square-enix.com'),
('Ubisoft', 'https://www.ubisoft.com'),
('Electronic Arts', 'https://www.ea.com'),
('Sony Interactive Entertainment', 'https://www.playstation.com'),
('Nintendo', 'https://www.nintendo.com'),
('Bandai Namco', 'https://www.bandainamcoent.com'),
('Bethesda Game Studios', 'https://bethesda.net'),
('Rockstar Games', 'https://www.rockstargames.com'),
('Sega', 'https://www.sega.com'),
('Valve', 'https://www.valvesoftware.com'),
('Konami', 'https://www.konami.com');



INSERT INTO usuario (nombre, apellido, email, password, role, avatar_url, estado) VALUES 
('Kazuma', 'Kiryu', 'admin@kamurocho.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Administrador', NULL, 1),
('Goro', 'Majima', 'majima@kamurocho.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Administrador', NULL, 1),
('Ichiban', 'Kasuga', 'empleado1@kamurocho.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1),
('Taiga', 'Saejima', 'empleado2@kamurocho.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1),
('Daigo', 'Dojima', 'daigo@tojo.jp', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Administrador', NULL, 1),
('Makoto', 'Date', 'date@police.jp', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1),
('Haruka', 'Sawamura', 'haruka@idol.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1),
('Shun', 'Akiyama', 'akiyama@skyfinance.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Administrador', NULL, 1),
('Yuki', 'Cabaret', 'yuki@fourshiny.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1),
('Koyuki', 'Cabaret', 'koyuki@fourshiny.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1),
('Ryuji', 'Godai', 'ryuji@omiodai.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1),
('Tatsuya', 'Ukyo', 'tatsuya@dragonheat.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1),
('Shin', 'Amon', 'amon@secret.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Administrador', NULL, 1),
('Masato', 'Arakawa', 'arakawa@family.com', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1),
('Koichi', 'Adachi', 'adachi@police.jp', 'AQAAAAIAAYagAAAAEPT3AYJDMtzZnbFC9SVg37lZZM9AVEzcMYa10PkzBGCVcFN8y/CBv0cUesKSKAlVjQ==', 'Empleado', NULL, 1);



INSERT INTO cliente (dni, nombre, apellido, telefono, email) VALUES 
('35123456', 'Haruka', 'Sawamura', '2614556677', 'haruka@mail.com'),
('40987654', 'Date', 'Naoto', '2614889900', 'date@police.jp'),
('38456123', 'Akiyama', 'Shun', '2615332211', 'skyfinance@mail.com'),
('42111222', 'Yuki', 'Cabaret', '2616445566', 'yuki@fourshiny.com'),
('33221144', 'Koyuki', 'Cabaret', '2617554433', 'koyuki@fourshiny.com'),
('31222333', 'Taiga', 'Saejima', '2618990011', 'saejima@mail.com'),
('36444555', 'Masaru', 'Watase', '2619112233', 'watase@mail.com'),
('39777888', 'Nanba', 'Yu', '2613224455', 'nanba@mail.com'),
('41555666', 'Saeko', 'Mukoda', '2614335566', 'saeko@mail.com'),
('37000999', 'Joon-gi', 'Han', '2615446677', 'joongi@mail.com'),
('34888777', 'Tianyou', 'Zhao', '2616557788', 'zhao@mail.com'),
('43123789', 'Seonhee', 'Park', '2617668899', 'seonhee@mail.com'),
('32456789', 'Jo', 'Sawashiro', '2618779900', 'sawashiro@mail.com'),
('35789123', 'Yosuke', 'Tendo', '2619880011', 'tendo@mail.com'),
('38123456', 'Homare', 'Nishitani', '2612113344', 'nishitani@mail.com');



INSERT INTO juego (titulo, genero, descripcion, precio, stock, estado, desarrolladora_id) VALUES 
('Yakuza 0', 'Acción', 'Precuela de Kazuma Kiryu y Goro Majima en 1988.', 19.99, 15, 1, 1),
('Like a Dragon: Infinite Wealth', 'RPG', 'Aventura RPG en Hawái y Japón.', 59.99, 10, 1, 1),
('Cyberpunk 2077', 'RPG', 'Mundo abierto ambientado en Night City.', 39.99, 8, 1, 2),
('Resident Evil 4 Remake', 'Survival', 'Reimaginación del clásico de Capcom.', 49.99, 12, 1, 3),
('Elden Ring', 'Action RPG', 'Obra maestra de mundo abierto.', 59.99, 20, 1, 4),
('Final Fantasy VII Rebirth', 'RPG', 'Continuación del viaje de Cloud en un vasto mundo.', 69.99, 14, 1, 5),
('Assassin Creed Valhalla', 'Acción', 'La saga de los vikingos en Inglaterra.', 29.99, 25, 1, 6),
('FIFA 23 / EA Sports FC', 'Deportes', 'Simulador oficial de fútbol.', 34.99, 30, 1, 7),
('God of War Ragnarok', 'Acción', 'La aventura nórdica de Kratos y Atreus.', 59.99, 18, 1, 8),
('Super Mario Odyssey', 'Plataformas', 'Aventura tridimensional de Mario.', 49.99, 22, 1, 9),
('Tekken 8', 'Lucha', 'Nueva entrega de la legendaria saga de lucha.', 69.99, 16, 1, 10),
('Starfield', 'RPG Espacial', 'Exploración de más de 1000 planetas.', 49.99, 9, 1, 11),
('Grand Theft Auto V', 'Acción', 'Crimen y caos en Los Santos.', 29.99, 40, 1, 12),
('Yakuza Kiwami', 'Acción', 'Remake del primer Yakuza.', 19.99, 11, 1, 13),
('Half-Life 2', 'Shooter', 'Clásico revolucionario de Valve.', 9.99, 50, 1, 14);



INSERT INTO venta (cantidad, precio_total, juego_id, cliente_id, usuario_id) VALUES 
(1, 19.99, 1, 1, 3),
(2, 119.98, 2, 2, 3),
(1, 39.99, 3, 3, 4),
(1, 59.99, 5, 4, 3),
(1, 69.99, 6, 5, 5),
(3, 89.97, 7, 6, 6),
(1, 34.99, 8, 7, 7),
(1, 59.99, 9, 8, 8),
(2, 99.98, 10, 9, 9),
(1, 69.99, 11, 10, 10),
(1, 49.99, 12, 11, 11),
(4, 119.96, 13, 12, 12),
(1, 19.99, 14, 13, 13),
(5, 49.95, 15, 14, 14),
(2, 39.98, 1, 15, 15);



INSERT INTO resena (calificacion, comentario, estado, juego_id, cliente_id) VALUES 
(5, 'El mejor juego de la saga, historia excelente.', 1, 1, 1),
(5, 'Impresionante la cantidad de contenido.', 1, 2, 2),
(4, 'Gráficamente una bestia tras los parches.', 1, 3, 3),
(5, 'Obra maestra absoluta.', 1, 5, 4),
(5, 'El sistema de combate evolucionó genial.', 1, 6, 5),
(3, 'Entretenido pero muy repetitivo.', 1, 7, 6),
(4, 'Para jugar con amigos está muy bien.', 1, 8, 7),
(5, 'Historia y narrativa de 10.', 1, 9, 8),
(5, 'Divertidísimo para todas las edades.', 1, 10, 9),
(4, 'Muy buenos gráficos y fluidez.', 1, 11, 10),
(3, 'Le falta más vida en los planetas.', 1, 12, 11),
(5, 'Un clásico eterno.', 1, 13, 12),
(4, 'Excelente remasterización.', 1, 14, 13),
(5, 'Revolucionó los videojuegos para siempre.', 1, 15, 14),
(5, 'Imprescindible jugarlo al menos una vez.', 1, 4, 15);



INSERT INTO noticias (titulo, contenido, usuarioId) VALUES 
('¡Gran Apertura de Kamurocho Store!', 'Bienvenidos a la nueva plataforma oficial.', 1),
('Nuevos títulos de Ryu Ga Gotoku disponibles', 'Ya se encuentran cargados los últimos juegos.', 2),
('Descuentos de temporada en RPGs', 'Aprovechate de las rebajas especiales este mes.', 3),
('Mantenimiento programado del sistema', 'El servidor se actualizará el próximo martes.', 4),
('Llegada masiva de stock en físico', 'Reposición completa de títulos de PS5 y Xbox.', 5),
('Nuevas políticas para clientes VIP', 'Beneficios exclusivos en compras mayores a 3 juegos.', 6),
('Torneo relámpago de Tekken 8', 'Inscripciones abiertas con premios en store.', 7),
('Actualización de seguridad en cuentas', 'Recordá cambiar tu contraseña periódicamente.', 8),
('Nuevos métodos de pago aceptados', 'Ahora podés abonar con billeteras virtuales.', 9),
('Entrevista exclusiva con desarrolladores', 'Hablamos con los creadores de Cyberpunk.', 10),
('Cierre de servidores antiguos', 'Algunos títulos multijugador dejarán de operar.', 11),
('Lanzamientos anticipados para empleados', 'Beneficios internos para el equipo.', 12),
('Se suman nuevas desarrolladoras indie', 'Más de 20 estudios se unen a la plataforma.', 13),
('Reporte de ventas récord trimestral', '¡Gracias a todos por el apoyo constante!', 14),
('Próximas funciones en la web', 'Pronto tendremos soporte para reseñas con fotos.', 15);


