import { apiRequest } from './apiClient'
import type { CategoryDto, CreateCategoryRequest, UpdateCategoryRequest } from './models'

export async function getCategories(): Promise<CategoryDto[]> {
  return apiRequest<CategoryDto[]>('/categories')
}

export async function createCategory(data: CreateCategoryRequest): Promise<string> {
  return apiRequest<string>('/categories', {
    method: 'POST',
    body: JSON.stringify(data),
  })
}

export async function updateCategory(id: string, data: UpdateCategoryRequest): Promise<void> {
  await apiRequest<void>(`/categories/${id}`, {
    method: 'PUT',
    body: JSON.stringify(data),
  })
}

export async function deleteCategory(id: string): Promise<void> {
  await apiRequest<void>(`/categories/${id}`, {
    method: 'DELETE',
  })
}
