import random
import hashlib
import sympy

def generate_large_prime():
    while True:
        p = random.randint(10**6, 10**7)
        if sympy.isprime(p):
            return p

# Знайдемо первісний корінь g за модулем p
def find_primitive_root(p):
    for g in range(2, p):
        if sympy.isprime(g):
            if pow(g, (p - 1) // 2, p) != 1:
                return g

def generate_private_key(p):
    private_key = random.randint(2, p - 2)
    h = pow(g, private_key, p)
    return private_key, h

def calculate_hash(message):
    hashed_message = hashlib.sha256(message.encode()).hexdigest()
    return int(hashed_message, 16)

# Обчислюємо цифровий підпис для хешу
def calculate_digital_signature(hashed_message, private_key):
    signature = pow(hashed_message, private_key, p)
    return signature

def write_signature_to_file(signature):
    with open('digital_signature.txt', 'w') as file:
        file.write(str(signature))

# Збереження хеша у файл
def write_hash_to_file(hashed_message):
    with open('hashed_message.txt', 'w') as file:
        file.write(str(hashed_message))

if __name__ == "__main__":
    p = generate_large_prime()
    g = find_primitive_root(p)
    private_key, h = generate_private_key(p)
    print(f"p: {p}, g: {g}, h: {h} \nPrivate Key: {private_key}")

    message = "Hello world"
    hashed_message = calculate_hash(message)
    print(f"Hashed Message: {hashed_message}")

    write_hash_to_file(hashed_message)  # Зберегти хеш у файл

    signature = calculate_digital_signature(hashed_message, private_key)
    print(f"Digital Signature: {signature}")

    write_signature_to_file(signature)
