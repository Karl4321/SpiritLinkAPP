"""Базовые абстракции классификаторов."""

import logging
from abc import ABC, abstractmethod

import torch

logger = logging.getLogger(__name__)

DEVICE = torch.device("cuda" if torch.cuda.is_available() else "cpu")


def _log_device() -> None:
    if DEVICE.type != "cuda":
        logger.info("Инференс на CPU")
        return
    major, minor = torch.cuda.get_device_capability(0)
    logger.info("Инференс на GPU: %s (sm_%d%d)", torch.cuda.get_device_name(0), major, minor)
    if f"sm_{major}{minor}" not in torch.cuda.get_arch_list():
        logger.warning(
            "Установленный PyTorch %s не содержит ядер для sm_%d%d — возможны ошибки CUDA. "
            "Установите сборку PyTorch под вашу видеокарту (для RTX 50xx — cu128).",
            torch.__version__, major, minor,
        )


_log_device()


class BaseClassifier(ABC):
    """Классификатор запросов: 1 — вредоносный, 0 — безопасный."""

    name: str = ""

    @abstractmethod
    def classify(self, text: str) -> int:
        ...

    def get_name(self) -> str:
        return self.name or type(self).__name__
