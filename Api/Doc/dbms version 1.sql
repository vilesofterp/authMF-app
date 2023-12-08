-- VAS
CREATE TABLE "user"
(
    id bigserial NOT NULL,
    name varchar(100)  NOT NULL,
    nickname varchar(50)  NOT NULL,
    email varchar(256)  NOT NULL,
    email_alternative  varchar(256)  NOT NULL,
    password varchar(100)  NOT NULL,
    gender varchar(1) ,
    language varchar(10)  NOT NULL,
    active integer NOT NULL default 0,
    id_time_zone bigint,
    phone varchar(50) ,
    profile integer NOT NULL DEFAULT 1,
    mfa integer NOT NULL DEFAULT 1,
    token_auth_public text not null,
    token_auth_private text not null,
    global_logout timestamp,
    CONSTRAINT user_pk PRIMARY KEY (id),
    CONSTRAINT user_uk_email UNIQUE (email),
    CONSTRAINT user_uk_email_alternative UNIQUE (email_alternative),
    CONSTRAINT user_uk_nickname UNIQUE (nickname)
);

