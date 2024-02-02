import random
from sympy import isprime

# Функція для генерації великого простого числа
def generate_large_prime(start, end):
    while True:
        num = random.randint(start, end)
        if isprime(num):
            return num

# Генеруємо два великих простих числа p і q
p = generate_large_prime(10**5, 10**6)
q = generate_large_prime(10**5, 10**6)


# Обчислюємо модуль n
n = p * q

# Генеруємо випадкове число x
x = random.randint(2, n-1)

# Функція BBS для генерації бітів ключа
def BBS(x):
    while True:
        x = (x*x) % n
        yield x % 2

# Функція для шифрування/дешифрування повідомлення
def encrypt_decrypt(message, generator):
    return ''.join(chr(ord(c) ^ next(generator)) for c in message)

# Створюємо генератор ключа
key_generator = BBS(x)

message = "Hello, world!"

# Шифруємо повідомлення
encrypted_message = encrypt_decrypt(message, key_generator)
print(f"Зашифроване повідомлення: {encrypted_message}")

# Для дешифрування ми знову створюємо генератор ключа (оскільки в Python генератори можна використовувати тільки один раз)
key_generator = BBS(x)

# Дешифруємо повідомлення
decrypted_message = encrypt_decrypt(encrypted_message, key_generator)
print(f"Розшифроване повідомлення: {decrypted_message}")