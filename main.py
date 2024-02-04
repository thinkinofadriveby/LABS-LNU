# 1

# Приклад використання map в Python з лямбда-функцією та лексичним замиканням
numbers = [1, 2, 3, 4, 5]

mapped_result = list(map(lambda x: x * 2, numbers))
print(f'Output list: {mapped_result}')  # Результат: [2, 4, 6, 8, 10]

# 2

# Приклад використання filter в Python з лямбда-функцією та лексичним замиканням
numbers = [1, 2, 3, 4, 5, 6]

filtered_result = list(filter(lambda x: x % 2 == 0, numbers))
print(f'Outlist list: {filtered_result}')  # Результат: [2, 4, 6]
