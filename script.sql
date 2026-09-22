CREATE DATABASE IF NOT EXISTS gamestore;
USE gamestore;

CREATE TABLE desarrolladora (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    web VARCHAR(150) NULL
);

CREATE TABLE usuario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL,
    avatar_url VARCHAR(255) NULL,
    estado TINYINT(1) DEFAULT 1
);

CREATE TABLE cliente (
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






