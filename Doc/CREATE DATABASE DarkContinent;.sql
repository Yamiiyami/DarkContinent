CREATE DATABASE DarkContinent;
GO
USE DarkContinent;
GO

-- 1. BẢNG DANH MỤC CÁC LOẠI PET 
CREATE TABLE pet_types (
    id INT PRIMARY KEY,
    name NVARCHAR(200) NOT NULL,
    description NVARCHAR(500) NULL,
    
    max_hunger INT NOT NULL DEFAULT 100 CHECK (max_hunger >= 0),
    max_energy INT NOT NULL DEFAULT 100 CHECK (max_energy >= 0),
    max_happiness INT NOT NULL DEFAULT 100 CHECK (max_happiness >= 0),
    
    sleep_energy_gain_per_hour SMALLINT NOT NULL DEFAULT 10,
    sleep_hunger_cost_per_hour SMALLINT NOT NULL DEFAULT 5,

    is_alive BIT NOT NULL DEFAULT 1,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME NOT NULL DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);

-- 2. BẢNG LƯU TRỮ THÔNG TIN PET 
CREATE TABLE pets (
    id INT PRIMARY KEY,
    name NVARCHAR(200) NOT NULL,
    type INT NOT NULL,

    age INT NOT NULL DEFAULT 0 CHECK (age >= 0),
    hunger DECIMAL(5,2) NOT NULL DEFAULT 100.00, 
    energy DECIMAL(5,2) NOT NULL DEFAULT 100.00,
    happiness DECIMAL(5,2) NOT NULL DEFAULT 100.00,
    
    level INT NOT NULL DEFAULT 1 CHECK (level >= 1),
    exp INT NOT NULL DEFAULT 0 CHECK (exp >= 0),
    born_at DATETIME NOT NULL DEFAULT GETDATE(),
    died_at DATETIME NULL, 
    is_alive BIT NOT NULL DEFAULT 1,

    last_calculated_at DATETIME NOT NULL DEFAULT GETDATE(),

    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME NOT NULL DEFAULT GETDATE(),
    deleted_at DATETIME NULL, 

    FOREIGN KEY (type) REFERENCES pet_types(id)
);

-- 3. BẢNG DANH MỤC HOẠT ĐỘNG
CREATE TABLE activities (
    id INT PRIMARY KEY,
    name NVARCHAR(200) NOT NULL,
    description NVARCHAR(500) NULL,
    type TINYINT NOT NULL DEFAULT 0 CHECK (type >= 0 AND type <= 2), 
    
    exp_gain INT NOT NULL DEFAULT 0 CHECK (exp_gain >= 0),
    happiness_gain SMALLINT NOT NULL DEFAULT 0, 
    energy_cost SMALLINT NOT NULL DEFAULT 0,    
    hunger_cost SMALLINT NOT NULL DEFAULT 0,    

    min_energy_required TINYINT NOT NULL DEFAULT 0 CHECK (min_energy_required >= 0),

    is_active BIT NOT NULL DEFAULT 1,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME NOT NULL DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);

-- 4. BẢNG DANH MỤC THỨC ĂN
CREATE TABLE foods (
    id INT PRIMARY KEY,
    name NVARCHAR(200) NOT NULL,
    description NVARCHAR(500) NULL,

    hunger_gain SMALLINT NOT NULL DEFAULT 0 CHECK (hunger_gain >= 0), 
    energy_gain SMALLINT NOT NULL DEFAULT 0 CHECK (energy_gain >= 0),
    happiness_gain SMALLINT NOT NULL DEFAULT 0 CHECK (happiness_gain >= 0),

    is_active BIT NOT NULL DEFAULT 1,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME NOT NULL DEFAULT GETDATE(),
    deleted_at DATETIME NULL
);

-- 5. NHẬT KÝ HOẠT ĐỘNG 
CREATE TABLE pet_actions_log (
    id BIGINT IDENTITY(1,1) PRIMARY KEY, 
    pet_id INT NOT NULL,
    activity_id INT NOT NULL, 
    performed_at DATETIME NOT NULL DEFAULT GETDATE(),
    hunger_before DECIMAL(5,2),
    hunger_after DECIMAL(5,2),
    energy_before DECIMAL(5,2),
    energy_after DECIMAL(5,2),
    happiness_before DECIMAL(5,2),
    happiness_after DECIMAL(5,2),

    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME NOT NULL DEFAULT GETDATE(),
    
    FOREIGN KEY (pet_id) REFERENCES pets(id),
    FOREIGN KEY (activity_id) REFERENCES activities(id)
);

-- 6. NHẬT KÝ CHO ĂN 
CREATE TABLE pet_feed_logs (
    id BIGINT IDENTITY(1,1) PRIMARY KEY, 
    pet_id INT NOT NULL,
    food_id INT NOT NULL,
    hunger_before DECIMAL(5,2),
    hunger_after DECIMAL(5,2),
    energy_before DECIMAL(5,2),
    energy_after DECIMAL(5,2),
    was_overfed BIT NOT NULL DEFAULT 0, 
    fed_at DATETIME NOT NULL DEFAULT GETDATE(),

    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (pet_id) REFERENCES pets(id),
    FOREIGN KEY (food_id) REFERENCES foods(id)
);

-- 7. NHẬT KÝ NGỦ NGHỈ 
CREATE TABLE pet_sleep_logs (
    id BIGINT IDENTITY(1,1) PRIMARY KEY, 
    pet_id INT NOT NULL,
    start_at DATETIME NOT NULL DEFAULT GETDATE(),
    end_at DATETIME NULL, 
    energy_before DECIMAL(5,2),
    energy_after DECIMAL(5,2),
    duration_in_minutes INT NOT NULL DEFAULT 0,
    is_full_sleep BIT NOT NULL DEFAULT 0, 

    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    updated_at DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (pet_id) REFERENCES pets(id)
);
